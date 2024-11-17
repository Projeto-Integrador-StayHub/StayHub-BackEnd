using Microsoft.EntityFrameworkCore;
using StayHub_BackEnd.Data;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.Services.Hospede
{
    public class HospedeService : IHospede
    {
        private readonly AppDbContext _context;
        public HospedeService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<List<HospedeModel>>> ListarHospedes()
        {
            ResponseModel<List<HospedeModel>> resposta = new ResponseModel<List<HospedeModel>>();
            try
            {
                var autores = await _context.Hospedes.ToListAsync();

                resposta.Dados = autores;
                resposta.Mensagem = "Lista de hospedes retornada com sucesso!";
                resposta.Status = true;

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
        public async Task<ResponseModel<HospedeModel>> BuscarHospede(int idHospede)
        {
            ResponseModel<HospedeModel> resposta = new ResponseModel<HospedeModel>();
            try
            {
                var hospede = await _context.Hospedes.FirstOrDefaultAsync(hospedeBanco => hospedeBanco.Id == idHospede);

                if (hospede == null)
                {
                    resposta.Mensagem = "Hospede não localizado!";
                    return resposta; 
                }

                resposta.Dados = hospede;
                resposta.Mensagem = "Hospede encontrado com sucesso!";
                resposta.Status = true;
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

        public async Task<ResponseModel<List<HospedeModel>>> CriarHospede(HospedeDto hospedeDto)
        {
            ResponseModel<List<HospedeModel>> resposta = new ResponseModel<List<HospedeModel>>();
            try
            {
                var hospede = new HospedeModel
                {
                    Nome = hospedeDto.Nome,
                    Email = hospedeDto.Email,
                    Senha = hospedeDto.Senha,
                    Telefone = hospedeDto.Telefone,
                    Nascimento = hospedeDto.Nascimento,
                    Cpf = hospedeDto.Cpf,
                    Endereco = hospedeDto.Endereco
                };
                _context.Add(hospede);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.Hospedes.ToListAsync();
                resposta.Mensagem = "Hospede criado com sucesso!";
                resposta.Status = true;

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

        public async Task<ResponseModel<HospedeModel>> EditarHospede(int idHospede, HospedeDto hospedeDto)
        {
            ResponseModel<HospedeModel> resposta = new ResponseModel<HospedeModel>();

            try
            {
                var hospede = await _context.Hospedes.FirstOrDefaultAsync(hospedeBanco => hospedeBanco.Id == idHospede);

                if (hospede == null)
                {
                    resposta.Mensagem = "Hospede não localizado!";
                    return resposta;
                }

                hospede.Nome = hospedeDto.Nome;
                hospede.Email = hospedeDto.Email;
                hospede.Senha = hospedeDto.Senha;
                hospede.Telefone = hospedeDto.Telefone;
                hospede.Nascimento = hospedeDto.Nascimento;
                hospede.Cpf = hospedeDto.Cpf;
                hospede.Endereco = hospedeDto.Endereco;

                _context.Update(hospede);
                await _context.SaveChangesAsync();

                resposta.Dados = hospede;
                resposta.Mensagem = "Hospede editado com sucesso!";
                resposta.Status = true;

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

        public async Task<ResponseModel<List<HospedeModel>>> ExcluirHospede(int idHospede)
        {
            ResponseModel<List<HospedeModel>> resposta = new ResponseModel<List<HospedeModel>>();
            
            try
            {
                var hospede = await _context.Hospedes.FirstOrDefaultAsync(hospedeBanco => hospedeBanco.Id == idHospede);
                if (hospede == null)
                {
                    resposta.Mensagem = "Hospede não localizado!";
                    return resposta;
                }
                _context.Remove(hospede);
                await _context.SaveChangesAsync();
                resposta.Dados = await _context.Hospedes.ToListAsync();
                resposta.Mensagem = "Hospede excluído com sucesso!";
                resposta.Status = true;
                return resposta;

            }
            catch(Exception ex)
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
    }
}
