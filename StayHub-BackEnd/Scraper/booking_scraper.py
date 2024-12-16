from playwright.sync_api import sync_playwright
import pandas as pd
import urllib.parse
import requests

def main():
    with sync_playwright() as p:
        checkin_date = '2025-02-23'
        checkout_date = '2025-02-24'

        cidades = [
            # Paraná
            "Curitiba", "Campo Mourão", "Morretes", "Ilha do Mel", "Londrina", "Foz do Iguaçu", "Cascavel",
            "Ponta Grossa",
            "Vale do Ivaí",

            # Santa Catarina
            "Florianópolis", "Balneário Camboriú", "Blumenau", "Joinville", "Bombinhas", "Itapema", "São Joaquim",
            "Pomerode",
            "Penha", "Garopaba", "Urubici", "Praia Grande", "Imbituba",

            # Rio Grande do Sul
            "Porto Alegre", "Gramado", "Canela", "Bento Gonçalves", "Caxias do Sul", "Pelotas", "Novo Hamburgo",
            "São Miguel das Missões", "Torres", "Cambará do Sul", "Santa Maria", "Rio Grande", "Bagé",
            "Santana do Livramento", "São Francisco de Paula"
        ]

        browser = p.chromium.launch(headless=False)
        page = browser.new_page()

        all_hotels_list = []

        for cidade in cidades:
            encoded_destination = urllib.parse.quote(cidade)
            page_url = f'https://www.booking.com/searchresults.pt-br.html?ss={encoded_destination}&checkin={checkin_date}&checkout={checkout_date}&group_adults=2&no_rooms=1&group_children=0'

            page.goto(page_url, timeout=60000)
            page.wait_for_load_state("networkidle")

            hotels = page.locator('[data-testid="property-card"]').all()
            print(f'Tem: {len(hotels)} hoteis em {cidade}.')

            count = 0
            for hotel in hotels:
                hotel_dict = {}
                
                hotel_dict['NomeQuarto'] = hotel.locator('[data-testid="title"]').inner_text()

                nome_quarto_elements = hotel.locator("//h4[contains(@class, 'abf093bdfe')]").all()
                hotel_dict['Descricao'] = nome_quarto_elements[0].inner_text() if nome_quarto_elements else "Nome do quarto indisponível"

                price_locator = hotel.locator('[data-testid="price-and-discounted-price"]')
                if price_locator.count() > 0 and price_locator.first.is_visible():
                    price_text = price_locator.first.inner_text()
                    price_text = price_text.replace('\u00A0', ' ')
                    hotel_dict['Preco'] = price_text.replace('R$', '')
                else:
                    hotel_dict['Preco'] = "Preço Indisponível"

                hotel_dict['CapacidadePessoas'] = 2
                hotel_dict['Disponibilidade'] = True
                hotel_dict['Comodidades'] = 'Indisponível'
                hotel_dict['Endereco'] = f'Centro, {cidade}'
                hotel_dict['DonoId'] = 0

                all_hotels_list.append(hotel_dict)

        # Solução temporária para salvar os dados em um arquivo csv
        df = pd.DataFrame(all_hotels_list)
        df.to_csv('hotels_list.csv', index=False)
        
        # Subir os dados para o banco de dados está dando erro
        # api_url = "http://localhost:5000/api/Quarto/CriarQuarto"
        # headers = {'Content-Type': 'application/json'}

        # for hotel_data in all_hotels_list:
        #     response = requests.post(api_url, json=hotel_data, headers=headers)
        #     if response.status_code == 200:
        #         print(f"Quarto {hotel_data['NomeQuarto']} criado com sucesso.")
        #     else:
        #         print(f"Erro ao criar quarto {hotel_data['NomeQuarto']}: {response.text}")


        browser.close()

if __name__ == '__main__':
    main()