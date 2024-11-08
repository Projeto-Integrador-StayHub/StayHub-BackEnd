using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;
using StayHub_BackEnd.Services.Admin;
using StayHub_BackEnd.Services.DonoHotel;

namespace StayHub_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonoHotelController : ControllerBase
    {
        private readonly IDonoHotel _iDonoHotel;
        public DonoHotelController(IDonoHotel iDonoHotel)
        {
            _iDonoHotel = iDonoHotel;
        }

        [HttpGet("ListarDonos")]


        [HttpGet("BuscarDono/{idDono}")]


        [HttpPost("CriarDono")]


        [HttpPut("AtualizarDono/{idDono}")]

        [HttpDelete("ExcluirDono/{idDono}")]
        
    }
}
