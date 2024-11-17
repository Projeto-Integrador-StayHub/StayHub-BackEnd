using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.Services.Reserva
{
    public interface IReserva
    {
        Task<ResponseModel<List<ReservaModel>>> ListarReservas();
        Task<ResponseModel<ReservaModel>> BuscarReserva(int idReserva);
        Task<ResponseModel<List<ReservaModel>>> CriarReserva(ReservaDto reservaDto);
        Task<ResponseModel<ReservaModel>> EditarReserva(int idReserva, ReservaDto reservaDto);
        Task<ResponseModel<List<ReservaModel>>> ExcluirReserva(int idReserva);
    }
}
