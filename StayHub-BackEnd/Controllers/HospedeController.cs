﻿using Microsoft.AspNetCore.Mvc;
using StayHub_BackEnd.Services.Hospede;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace StayHub_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospedeController : ControllerBase
    {
        private readonly IHospede _hospedeInterface;
        public HospedeController(IHospede iHospede)
        {
            _hospedeInterface = iHospede;
        }

        [HttpGet("ListarHospedes")]
        public async Task<ActionResult<ResponseModel<List<HospedeModel>>>> ListarHospedes()
        {
            var hospedes = await _hospedeInterface.ListarHospedes();
            return Ok(hospedes);
        }

        [HttpGet("BuscarHospede/{idHospede}")]
        public async Task<ActionResult<ResponseModel<HospedeModel>>> BuscarHospedes(int idHospede)
        {
            var hospede = await _hospedeInterface.BuscarHospede(idHospede);
            return Ok(hospede);
        }

        [HttpPost("CriarHospede/")]
        public async Task<ActionResult<ResponseModel<List<HospedeModel>>>> CriarHospede(HospedeDto hospedeDto)
        {
            var hospede = await _hospedeInterface.CriarHospede(hospedeDto);
            return Ok(hospede);
        }

        [HttpPut("EditarHospede/{idHospede}")]
        public async Task<ActionResult<ResponseModel<List<HospedeModel>>>> EditarHospede(int idHospede, HospedeDto hospedeDto)
        {
            var hospede = await _hospedeInterface.EditarHospede(idHospede,hospedeDto);
            return Ok(hospede);
        }

        [HttpDelete("ExcluirHospede/{idHospede}")]
        public async Task<ActionResult<ResponseModel<List<HospedeModel>>>> ExcluirHospede(int idHospede)
        {
            var hospede = await _hospedeInterface.ExcluirHospede(idHospede);
            return Ok(hospede);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email e senha são obrigatórios.");
            }

            var result = await _hospedeInterface.ValidateLoginAsync(request.Email, request.Password);
            if (result == null)
            {
                return Unauthorized("Credenciais inválidas.");
            }

            return Ok(new { Message = "Login realizado com sucesso!" });
        }
    }
}
