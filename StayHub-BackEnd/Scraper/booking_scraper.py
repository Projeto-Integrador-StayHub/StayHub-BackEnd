import pyodbc
import pandas as pd
from datetime import datetime
import urllib
from playwright.sync_api import sync_playwright
import json
import os

# Função para carregar as configurações do appsettings.json
def load_config():
    config_path = os.path.join(os.path.dirname(__file__), '..', 'appsettings.json')
    with open(config_path, 'r') as config_file:
        config = json.load(config_file)
    return config

# Função para construir a string de conexão para o pyodbc
def build_connection_string(config):
    connection_string = config['ConnectionStrings']['DefaultConnection']
    connection_params = dict(param.strip().split('=') for param in connection_string.split(';') if param)
    return (
        f"DRIVER={{ODBC Driver 17 for SQL Server}};"
        f"SERVER={connection_params['server']};"
        f"DATABASE={connection_params['database']};"
        f"Trusted_Connection=yes;"
    )


# Function to get or create owner and return owner ID
def get_or_create_owner(conn, owner_dto):
    cursor = conn.cursor()
    cursor.execute("SELECT Id FROM dbo.DonosHoteis WHERE Email = ?", owner_dto['Email'])
    owner = cursor.fetchone()

    if owner:
        return owner[0]
    else:
        cursor.execute("""
            INSERT INTO dbo.DonosHoteis (Nome, Email, Senha, Telefone, Nascimento, Cpf, Endereco)
            VALUES (?, ?, ?, ?, ?, ?, ?)
        """, owner_dto['Nome'], owner_dto['Email'], owner_dto['Senha'], owner_dto['Telefone'],
           owner_dto['Nascimento'], owner_dto['Cpf'], owner_dto['Endereco'])
        conn.commit()

        cursor.execute("SELECT Id FROM dbo.DonosHoteis WHERE Email = ?", owner_dto['Email'])
        owner_id = cursor.fetchone()[0]
        return owner_id

# Function to check if hotel data already exists in SQL Server
def hotel_exists(conn, hotel_dict):
    cursor = conn.cursor()
    cursor.execute("""
        SELECT COUNT(*)
        FROM dbo.Quartos
        WHERE NomeQuarto = ? AND Endereco = ? AND Cidade = ?
    """, hotel_dict['NomeQuarto'], hotel_dict['Endereco'], hotel_dict['Cidade'])
    count = cursor.fetchone()[0]
    return count > 0

# Function to insert data into SQL Server
def insert_data_to_sql_server(df, owner_id, conn):
    cursor = None
    new_rooms_count = 0
    try:
        cursor = conn.cursor()
        for index, row in df.iterrows():
            hotel_dict = row.to_dict()
            if not hotel_exists(conn, hotel_dict):
                cursor.execute("""
                    INSERT INTO dbo.Quartos (NomeQuarto, Descricao, Preco, CapacidadePessoas, Disponibilidade,
                                             Comodidades, Endereco, DonoId, Estado, Cidade)
                    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
                """, row['NomeQuarto'], row['Descricao'], row['Preco'], row['CapacidadePessoas'],
                   row['Disponibilidade'], row['Comodidades'], row['Endereco'], owner_id, row['Estado'], row['Cidade'])
                new_rooms_count += 1
            else:
                print(f"Hotel '{row['NomeQuarto']}' já existe no banco de dados.")
        conn.commit()
    except Exception as e:
        print(f"An error occurred: {e}")
    finally:
        if cursor:
            cursor.close()
    return new_rooms_count

# Static owner DTO
owner_dto = {
    'Nome': 'Scraping',
    'Email': 'scraping@mail',
    'Senha': '123',
    'Telefone': '1234567890',
    'Nascimento': datetime(1980, 1, 1),
    'Cpf': '12345678901',
    'Endereco': '123'
}

def main():
    # Carregar as configurações do appsettings.json
    config = load_config()
    connection_string = build_connection_string(config)

    # Estabelecer conexão com o banco de dados
    conn = pyodbc.connect(connection_string)

    # Obter o owner_id antes do loop principal
    owner_id = get_or_create_owner(conn, owner_dto)

    with sync_playwright() as p:
        checkin_date = '2025-02-23'
        checkout_date = '2025-02-24'

        cidades = [
            # Paraná
            "Curitiba", "Campo Mourão"
        ]

        browser = p.chromium.launch(headless=False)
        page = browser.new_page()

        all_hotels_list = []

        for cidade in cidades:
            encoded_destination = urllib.parse.quote(cidade)
            page_url = (
                f'https://www.booking.com/searchresults.pt-br.html?ss={encoded_destination}'
                f'&checkin={checkin_date}&checkout={checkout_date}'
                '&group_adults=2&no_rooms=1&group_children=0'
            )

            page.goto(page_url, timeout=60000)
            page.wait_for_load_state("networkidle")

            hotels = page.locator('[data-testid="property-card"]').all()
            print(f'Tem: {len(hotels)} hotéis em {cidade}.')

            for hotel in hotels:
                hotel_dict = {}

                hotel_dict['NomeQuarto'] = hotel.locator('[data-testid="title"]').inner_text().strip()

                nome_quarto_elements = hotel.locator("//h4[contains(@class, 'abf093bdfe')]").all()
                hotel_dict['Descricao'] = (
                    nome_quarto_elements[0].inner_text().strip()
                    if nome_quarto_elements else "Nome do quarto indisponível"
                )

                price_locator = hotel.locator('[data-testid="price-and-discounted-price"]')
                if price_locator.count() > 0 and price_locator.first.is_visible():
                    price_text = price_locator.first.inner_text()
                    price_text = price_text.replace('\u00A0', ' ').replace('.', '').replace(',', '.').strip()
                    hotel_dict['Preco'] = float(price_text.replace('R$', ''))
                else:
                    hotel_dict['Preco'] = 0.0  # Valor padrão

                hotel_dict['CapacidadePessoas'] = 2
                hotel_dict['Disponibilidade'] = True
                hotel_dict['Comodidades'] = 'Indisponível'
                hotel_dict['Estado'] = 'Paraná'
                hotel_dict['Cidade'] = cidade
                hotel_dict['Endereco'] = f'Centro, {cidade}'
                hotel_dict['DonoId'] = owner_id  # Agora o owner_id está definido

                all_hotels_list.append(hotel_dict)

        browser.close()

    df = pd.DataFrame(all_hotels_list)
    new_rooms_count = insert_data_to_sql_server(df, owner_id, conn)
    conn.close()

    # Retornar mensagem JSON com o valor e a quantidade adicionada
    response = {
        "message": "Scraping completed successfully!",
        "new_rooms_count": new_rooms_count
    }
    print(json.dumps(response, indent=4))

if __name__ == '__main__':
    main()








