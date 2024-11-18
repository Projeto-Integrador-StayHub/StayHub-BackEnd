using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.Services.Hospede
{
    public interface IHospede
    {
        Task<ResponseModel<List<HospedeModel>>> ListarHospedes();
        Task<ResponseModel<HospedeModel>> BuscarHospede(int idHospede);
        Task<ResponseModel<List<HospedeModel>>> CriarHospede(HospedeDto hospedeDto);
        Task<ResponseModel<HospedeModel>> EditarHospede(int idHospede, HospedeDto hospedeDto);
        Task<ResponseModel<List<HospedeModel>>> ExcluirHospede(int idHospede);
        Task<HospedeModel> ValidateLoginAsync(string email, string senha);
    }
}
