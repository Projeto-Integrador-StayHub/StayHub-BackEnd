using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.Services.DonoHotel
{
    public interface IDonoHotel
    {
        Task<ResponseModel<List<DonoHotelModel>>> ListarDonos();
        Task<ResponseModel<DonoHotelModel>> BuscarDono(int idDono);
        Task<ResponseModel<DonoHotelModel>> BuscarDonoPorQuarto(int idQuarto);
        Task<ResponseModel<List<DonoHotelModel>>> CriarDono(DonoHotelDto donoHotelDto);
        Task<ResponseModel<DonoHotelModel>> EditarDono(int idDono, DonoHotelDto donoHotelDto);
        Task<ResponseModel<List<DonoHotelModel>>> ExcluirDono(int idDono);
        Task<DonoHotelModel> ValidateLoginAsync(string email, string senha);
    }
}
