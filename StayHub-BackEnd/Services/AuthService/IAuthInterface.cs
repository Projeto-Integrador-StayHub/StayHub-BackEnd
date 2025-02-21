using StayHub_BackEnd.Models;
using StayHub_BackEnd.Dtos;

namespace StayHub_BackEnd.Services.AuthService
{
    public interface IAuthInterface
    {
        Task<ResponseModel<HospedeCriacaoDto>> Registrar(HospedeCriacaoDto hospedeRegistro);
        Task<ResponseModel<string>> Login(HospedeLoginDto hospedeLogin);
    }
}
