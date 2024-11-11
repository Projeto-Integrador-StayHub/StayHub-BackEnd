using Microsoft.EntityFrameworkCore;
using StayHub_BackEnd.Data;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.Services.DonoHotel
{
    public class DonoHotelService : IDonoHotel
    {
        private readonly AppDbContext _context;
        public DonoHotelService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<DonoHotelModel>> BuscarDono(int idDono)
        {
            ResponseModel<DonoHotelModel> resposta = new ResponseModel<DonoHotelModel>();

            try
            {
                var dono = await _context.DonosHoteis.FirstOrDefaultAsync(x => x.Id == idDono); // se der bo, mudar "x" para ""

                if (dono == null)
                {
                    resposta.Mensagem = "Dono não foi encontrado!";
                    return resposta;
                }

                resposta.Dados = dono;
                resposta.Mensagem = "Dono encontrado!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<DonoHotelModel>> BuscarDonoPorQuarto(int idQuarto)
        {
            ResponseModel<DonoHotelModel> resposta = new ResponseModel<DonoHotelModel>();

            try
            {
                var quarto = await _context.Quartos.Include(a => a.Dono).FirstOrDefaultAsync(x => x.Id == idQuarto);

                if (quarto == null)
                {
                    resposta.Mensagem = "Dono não foi encontrado!";
                    return resposta;
                }

                resposta.Dados = quarto.Dono;
                resposta.Mensagem = "Dono encontrado!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<DonoHotelModel>>> CriarDono(DonoHotelDto donoHotelDto)
        {
            ResponseModel<List<DonoHotelModel>> resposta = new ResponseModel<List<DonoHotelModel>>();

            try
            {
                var dono = new DonoHotelModel()
                {
                    Nome = donoHotelDto.Nome,
                    Email = donoHotelDto.Email,
                    Senha = donoHotelDto.Senha,
                    Telefone = donoHotelDto.Telefone,
                    Nascimento = donoHotelDto.Nascimento,
                    Cpf = donoHotelDto.Cpf,
                    Endereco = donoHotelDto.Endereco
                };

                _context.Add(dono);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.DonosHoteis.ToListAsync();
                resposta.Mensagem = "Dono criado com sucesso!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<DonoHotelModel>> EditarDono(int idDono, DonoHotelDto donoHotelDto)
        {
            ResponseModel<DonoHotelModel> resposta = new ResponseModel<DonoHotelModel>();

            try
            {
                var dono = await _context.DonosHoteis.FirstOrDefaultAsync(x => x.Id == idDono);

                if (dono == null)
                {
                    resposta.Mensagem = "Dono não foi encontrado!";
                    return resposta;
                }

                dono.Nome = donoHotelDto.Nome;
                dono.Email = donoHotelDto.Email;
                dono.Senha = donoHotelDto.Senha;
                dono.Telefone = donoHotelDto.Telefone;
                dono.Nascimento = donoHotelDto.Nascimento;
                dono.Cpf = donoHotelDto.Cpf;
                dono.Endereco = donoHotelDto.Endereco;

                _context.Update(dono);
                await _context.SaveChangesAsync();

                resposta.Dados = dono;
                resposta.Mensagem = "Dono atualizado com sucesso!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<DonoHotelModel>>> ExcluirDono(int idDono)
        {
            ResponseModel<List<DonoHotelModel>> resposta = new ResponseModel<List<DonoHotelModel>>();

            try
            {
                var dono = await _context.Admins.FirstOrDefaultAsync(x => x.Id == idDono);

                if (dono == null)
                {
                    resposta.Mensagem = "Dono não foi encontrado!";
                    return resposta;
                }

                _context.Remove(dono);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.DonosHoteis.ToListAsync();
                resposta.Mensagem = "Dono deletado com sucesso!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<DonoHotelModel>>> ListarDonos()
        {
            ResponseModel<List<DonoHotelModel>> resposta = new ResponseModel<List<DonoHotelModel>>();

            try
            {
                var donos = await _context.DonosHoteis.ToListAsync();

                resposta.Dados = donos;
                resposta.Mensagem = "Lista de Donos de Hoteis/Quartos Retornada!";

                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

    }
}
