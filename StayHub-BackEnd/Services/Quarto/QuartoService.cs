using Microsoft.EntityFrameworkCore;
using StayHub_BackEnd.Data;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;
using StayHub_BackEnd.Services.Admin;

namespace StayHub_BackEnd.Services.Quarto
{
    public class QuartoService : IQuarto
    {
        private readonly AppDbContext _context;
        public QuartoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<QuartoModel>> BuscarQuarto(int idQuarto)
        {
            ResponseModel<QuartoModel> resposta = new ResponseModel<QuartoModel>();

            try
            {
                var quarto = await _context.Quartos.Include(d => d.Dono).FirstOrDefaultAsync(x => x.Id == idQuarto);

                if (quarto == null)
                {
                    resposta.Mensagem = "Quarto não foi encontrado!";
                    return resposta;
                }

                resposta.Dados = quarto;
                resposta.Mensagem = "Quarto encontrado!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = $"Erro: {ex.Message}";
                if (ex.InnerException != null)
                {
                    resposta.Mensagem += $" InnerException: {ex.InnerException.Message}";
                }
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<QuartoModel>>> CriarQuarto(QuartoDto quartoDto)
        {
            ResponseModel<List<QuartoModel>> resposta = new ResponseModel<List<QuartoModel>>();

            try
            {

                var donoHotel = await _context.DonosHoteis
                .Where(d => d.Id == quartoDto.DonoId)
                .Select(d => new DonoHotelModel { Id = d.Id }) 
                .FirstOrDefaultAsync();
                if (donoHotel == null)
                {
                    resposta.Mensagem = "Dono não encontrado!";
                    resposta.Status = false;
                    return resposta;
                }

                var quarto = new QuartoModel()
                {
                    NomeQuarto = quartoDto.NomeQuarto,
                    Descricao = quartoDto.Descricao,
                    Preco = quartoDto.Preco,
                    CapacidadePessoas = quartoDto.CapacidadePessoas,
                    Disponibilidade = quartoDto.Disponibilidade,
                    Comodidades = quartoDto.Comodidades,
                    Endereco = quartoDto.Endereco,
                    DonoId = quartoDto.DonoId
                };

                _context.Add(quarto);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.Quartos.ToListAsync();
                resposta.Mensagem = "Quarto criado com sucesso!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = $"Erro: {ex.Message}";
                if (ex.InnerException != null)
                {
                    resposta.Mensagem += $" InnerException: {ex.InnerException.Message}";
                }
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<QuartoModel>> EditarQuarto(int idQuarto, QuartoDto quartoDto)
        {
            ResponseModel<QuartoModel> resposta = new ResponseModel<QuartoModel>();

            try
            {
                var quarto = await _context.Quartos.FirstOrDefaultAsync(x => x.Id == idQuarto);

                if (quarto == null)
                {
                    resposta.Mensagem = "Quarto não foi encontrado!";
                    return resposta;
                }

                quarto.NomeQuarto = quartoDto.NomeQuarto;
                quarto.Descricao = quartoDto.Descricao;
                quarto.Preco = quartoDto.Preco;
                quarto.CapacidadePessoas = quartoDto.CapacidadePessoas;
                quarto.Disponibilidade = quartoDto.Disponibilidade;
                quarto.Comodidades = quartoDto.Comodidades;
                quarto.Endereco = quartoDto.Endereco;

                _context.Update(quarto);
                await _context.SaveChangesAsync();

                resposta.Dados = quarto;
                resposta.Mensagem = "Quarto atualizado com sucesso!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = $"Erro: {ex.Message}";
                if (ex.InnerException != null)
                {
                    resposta.Mensagem += $" InnerException: {ex.InnerException.Message}";
                }
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<QuartoModel>>> ExcluirQuarto(int idQuarto)
        {
            ResponseModel<List<QuartoModel>> resposta = new ResponseModel<List<QuartoModel>>();

            try
            {
                var quarto = await _context.Quartos.FirstOrDefaultAsync(x => x.Id == idQuarto);

                if (quarto == null)
                {
                    resposta.Mensagem = "Quarto não foi encontrado!";
                    return resposta;
                }

                _context.Remove(quarto);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.Quartos.ToListAsync();
                resposta.Mensagem = "Quarto deletado com sucesso!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = $"Erro: {ex.Message}";
                if (ex.InnerException != null)
                {
                    resposta.Mensagem += $" InnerException: {ex.InnerException.Message}";
                }
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<QuartoModel>>> ListarQuartos()
        {
            ResponseModel<List<QuartoModel>> resposta = new ResponseModel<List<QuartoModel>>();

            try
            {
                var quartos = await _context.Quartos.Include(d => d.Dono).ToListAsync();

                resposta.Dados = quartos;
                resposta.Mensagem = "Lista de quartos Retornada!";

                return resposta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(resposta));
                resposta.Mensagem = $"Erro: {ex.Message}";
                if (ex.InnerException != null)
                {
                    resposta.Mensagem += $" InnerException: {ex.InnerException.Message}";
                }
                resposta.Status = false;
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(resposta));

                return resposta;
            }
        }
    }
}
