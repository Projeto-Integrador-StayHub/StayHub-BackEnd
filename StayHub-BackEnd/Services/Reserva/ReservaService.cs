using Microsoft.EntityFrameworkCore;
using StayHub_BackEnd.Data;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.Services.Reserva
{
    public class ReservaService : IReserva
    {
        private readonly AppDbContext _context;

        public ReservaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<ReservaModel>>> ListarReservas()
        {
            ResponseModel<List<ReservaModel>> resposta = new ResponseModel<List<ReservaModel>>();
            try
            {
                var reservas = await _context.Reservas.ToListAsync();
                resposta.Dados = reservas;
                resposta.Mensagem = "Lista de reservas retornada com sucesso!";
                resposta.Status = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<ReservaModel>> BuscarReserva(int idReserva)
        {
            ResponseModel<ReservaModel> resposta = new ResponseModel<ReservaModel>();
            try
            {
                var reserva = await _context.Reservas.FirstOrDefaultAsync(reservaBanco => reservaBanco.Id == idReserva);
                if (reserva == null)
                {
                    resposta.Mensagem = "Reserva não localizada!";
                    return resposta;
                }
                resposta.Dados = reserva;
                resposta.Mensagem = "Reserva encontrada com sucesso!";
                resposta.Status = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<ReservaModel>>> CriarReserva(ReservaDto reservaDto)
        {
            ResponseModel<List<ReservaModel>> resposta = new ResponseModel<List<ReservaModel>>();
            try
            {
                // Verifica se o hóspede já existe no banco de dados
                var hospede = await _context.Hospedes.FirstOrDefaultAsync(h => h.Id == reservaDto.HospedeId);
                if (hospede == null)
                {
                    resposta.Mensagem = "Hóspede não encontrado!";
                    resposta.Status = false;
                    return resposta;
                }

                var reserva = new ReservaModel
                {
                    HospedeId = reservaDto.HospedeId,
                    Nome = reservaDto.Nome,
                    Descricao = reservaDto.Descricao,
                    Entrada = reservaDto.Entrada,
                    Saida = reservaDto.Saida,
                    Preco = reservaDto.Preco,
                    Status = reservaDto.Status
                };
                _context.Add(reserva);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.Reservas.ToListAsync();
                resposta.Mensagem = "Reserva criada com sucesso!";
                resposta.Status = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<ReservaModel>> EditarReserva(int idReserva, ReservaDto reservaDto)
        {
            ResponseModel<ReservaModel> resposta = new ResponseModel<ReservaModel>();
            try
            {
                var reserva = await _context.Reservas.FirstOrDefaultAsync(reservaBanco => reservaBanco.Id == idReserva);

                if (reserva == null)
                {
                    resposta.Mensagem = "Reserva não localizada!";
                    resposta.Status = false;
                    return resposta;
                }

                reserva.Nome = reservaDto.Nome;
                reserva.Descricao = reservaDto.Descricao;
                reserva.Entrada = reservaDto.Entrada;
                reserva.Saida = reservaDto.Saida;
                reserva.Preco = reservaDto.Preco;
                reserva.Status = reservaDto.Status;
                reserva.HospedeId = reservaDto.HospedeId;

                await _context.SaveChangesAsync();
                resposta.Dados = reserva;

                resposta.Mensagem = "Reserva editada com sucesso!";
                resposta.Status = true;

                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<ReservaModel>>> ExcluirReserva(int idReserva)
        {
            ResponseModel<List<ReservaModel>> resposta = new ResponseModel<List<ReservaModel>>();
            try
            {
                var reserva = await _context.Reservas.FirstOrDefaultAsync(reservaBanco => reservaBanco.Id == idReserva);
                if (reserva == null)
                {
                    resposta.Mensagem = "Reserva não localizada!";
                    return resposta;
                }
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
                resposta.Dados = await _context.Reservas.ToListAsync();
                resposta.Mensagem = "Reserva excluída com sucesso!";
                resposta.Status = true;
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
