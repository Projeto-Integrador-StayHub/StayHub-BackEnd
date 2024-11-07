using Microsoft.EntityFrameworkCore;
using StayHub_BackEnd.Data;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.Services.Admin
{
    public class AdminService : IAdmin
    {
        private readonly AppDbContext _context;
        public AdminService(AppDbContext context)
        { 
            _context = context;
        }
        public async Task<ResponseModel<List<AdminModel>>> ListarAdmins()
        {
            ResponseModel<List<AdminModel>> resposta = new ResponseModel<List<AdminModel>>();

            try
            {
                var admins = await _context.Admins.ToListAsync();

                resposta.Dados = admins;
                resposta.Mensagem = "Lista de Administradores Retornada!";

                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<AdminModel>> BuscarAdmin(int idAdmin)
        {
            ResponseModel<AdminModel> resposta = new ResponseModel<AdminModel>();

            try
            {
                var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Id == idAdmin); // se der bo, mudar "x" para "adminBd"

                if (admin == null)
                {
                    resposta.Mensagem = "Administrador não foi encontrado!";
                    return resposta;
                }

                resposta.Dados = admin;
                resposta.Mensagem = "Administrador encontrado!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<AdminModel>>> CriarAdmin(AdminDto adminDto)
        {
            ResponseModel<List<AdminModel>> resposta = new ResponseModel<List<AdminModel>>();

            try
            {
                var admin = new AdminModel()
                {
                    Nome = adminDto.Nome,
                    Email = adminDto.Email,
                    Senha = adminDto.Senha
                };

                _context.Add(admin);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.Admins.ToListAsync();
                resposta.Mensagem = "Administrador criado com sucesso!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<AdminModel>> EditarAdmin(int idAdmin, AdminDto adminDto)
        {
            ResponseModel<AdminModel> resposta = new ResponseModel<AdminModel>();

            try
            {
                var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Id == idAdmin);

                if (admin == null)
                {
                    resposta.Mensagem = "Administrador não foi encontrado!";
                    return resposta;
                }

                admin.Nome = adminDto.Nome;
                admin.Email = adminDto.Email;
                admin.Senha = adminDto.Senha;

                _context.Update(admin);
                await _context.SaveChangesAsync();

                resposta.Dados = admin;
                resposta.Mensagem = "Administrador atualizado com sucesso!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<AdminModel>>> ExcluirAdmin(int idAdmin)
        {
            ResponseModel<List<AdminModel>> resposta = new ResponseModel<List<AdminModel>>();

            try
            {
                var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Id == idAdmin);

                if(admin == null)
                {
                    resposta.Mensagem = "Administrador não foi encontrado!";
                    return resposta;
                }

                _context.Remove(admin);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.Admins.ToListAsync();
                resposta.Mensagem = "Administrador deletado com sucesso!";
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
