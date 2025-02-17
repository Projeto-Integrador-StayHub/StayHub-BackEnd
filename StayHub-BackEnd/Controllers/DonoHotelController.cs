﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;
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
        public async Task<ActionResult<ResponseModel<List<DonoHotelModel>>>> ListarDonos()
        {
            var donos = await _iDonoHotel.ListarDonos();
            return Ok(donos);
        }

        [HttpGet("BuscarDonoId/{idDono}")]
        public async Task<ActionResult<ResponseModel<DonoHotelModel>>> BuscarDono(int idDono)
        {
            var dono = await _iDonoHotel.BuscarDono(idDono);
            return Ok(dono);
        }

        [HttpGet("BuscarDonoPorQaurtoId/{idQuarto}")]
        public async Task<ActionResult<ResponseModel<DonoHotelModel>>> BuscarDonoQuarto(int idQuarto)
        {
            var dono = await _iDonoHotel.BuscarDonoPorQuarto(idQuarto);
            return Ok(dono);
        }

        [HttpPost("CriarDono")]
        public async Task<ActionResult<ResponseModel<DonoHotelModel>>> CriarDono(DonoHotelDto donoHotelDto)
        {
            var dono = await _iDonoHotel.CriarDono(donoHotelDto);
            return Ok(dono);
        }

        [HttpPut("EditarDono/{idDono}")]
        public async Task<ActionResult<ResponseModel<DonoHotelModel>>> EditarDono(int idDono, DonoHotelDto donoHotelDto)
        {
            var dono = await _iDonoHotel.EditarDono(idDono, donoHotelDto);
            return Ok(dono);
        }

        [HttpDelete("ExcluirDono/{idDono}")]
        public async Task<ActionResult<ResponseModel<DonoHotelModel>>> ExcluirDono(int idDono)
        {
            var dono = await _iDonoHotel.ExcluirDono(idDono);
            return Ok(dono);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email e senha são obrigatórios.");
            }

            var result = await _iDonoHotel.ValidateLoginAsync(request.Email, request.Password);
            if (result == null)
            {
                return Unauthorized("Credenciais inválidas.");
            }

            return Ok(new { Message = "Login realizado com sucesso!" });
        }

    }
}
