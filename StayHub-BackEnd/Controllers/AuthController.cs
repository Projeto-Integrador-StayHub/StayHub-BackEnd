using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayHub_BackEnd.Services.AuthService;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Dtos;


namespace jwtRegisterLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _authInterface;
        public AuthController(IAuthInterface authInterface)
        {
                _authInterface = authInterface;
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login(HospedeLoginDto hospedeLogin)
        {

            var resposta = await _authInterface.Login(hospedeLogin); 
            return Ok(resposta);
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register(HospedeCriacaoDto hospedeRegister)
        {
            var resposta = await _authInterface.Registrar(hospedeRegister);
            return Ok(resposta);
        }


    }
}
