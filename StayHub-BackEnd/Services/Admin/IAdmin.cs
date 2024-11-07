using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.Services.Admin
{
    public interface IAdmin
    {
        Task<ResponseModel<List<AdminModel>>> ListarAdmins();
        Task<ResponseModel<AdminModel>> BuscarAdmin(int idAdmin);
        Task<ResponseModel<List<AdminModel>>> CriarAdmin(AdminDto adminDto);
        Task<ResponseModel<AdminModel>> EditarAdmin(int idAdmin, AdminDto adminDto);
        Task<ResponseModel<List<AdminModel>>> ExcluirAdmin(int idAdmin);
    }
}
