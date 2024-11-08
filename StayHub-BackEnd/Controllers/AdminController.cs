using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;
using StayHub_BackEnd.Services.Admin;

namespace StayHub_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _iadmin;
        public AdminController(IAdmin iadmin)
        {
            _iadmin = iadmin;
        }

        [HttpGet("ListarAdmins")]
        public async Task<ActionResult<ResponseModel<List<AdminModel>>>> ListarAdmins()
        {
            var admins = await _iadmin.ListarAdmins();
            return Ok(admins);
        }

        [HttpGet("BuscarAdminId/{idAdmin}")]
        public async Task<ActionResult<ResponseModel<AdminModel>>> BuscarAdminId(int idAdmin)
        {
            var admin = await _iadmin.BuscarAdmin(idAdmin);
            return Ok(admin);
        }

        [HttpPost("CriarAdmin")]
        public async Task<ActionResult<ResponseModel<AdminModel>>> CriarAdmin(AdminDto adminDto)
        {
            var admins = await _iadmin.CriarAdmin(adminDto);
            return Ok(admins);
        }

        [HttpPut("EditarAdmin/{idAdmin}")]
        public async Task<ActionResult<ResponseModel<AdminModel>>> EditarAdmin(int idAdmin, AdminDto adminDto)
        {
            var admins = await _iadmin.EditarAdmin(idAdmin, adminDto);
            return Ok(admins);
        }

        [HttpDelete("ExcluirAdmin/{idAdmin}")]
        public async Task<ActionResult<ResponseModel<AdminModel>>> ExcluirAdmin(int idAdmin)
        {
            var admins = await _iadmin.ExcluirAdmin(idAdmin);
            return Ok(admins);
        }

    }
}
