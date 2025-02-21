using Microsoft.EntityFrameworkCore;
using StayHub_BackEnd.Data;
using StayHub_BackEnd.Models;
using StayHub_BackEnd.Dtos;
using StayHub_BackEnd.Utils; // Added reference for PasswordHasher
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace StayHub_BackEnd.Services.AuthService
{
    public class AuthService : IAuthInterface
    {
        private readonly IConfiguration _config;

        private readonly AppDbContext _context;
        // Removed ISenhaInterface dependency
        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;

        }

        public async Task<ResponseModel<HospedeCriacaoDto>> Registrar(HospedeCriacaoDto hospedeRegistro)
        {
            ResponseModel<HospedeCriacaoDto> respostaServico = new ResponseModel<HospedeCriacaoDto>();

            try
            {
                if (!VerificaSeEmaileUsuarioJaExiste(hospedeRegistro))
                {
                    respostaServico.Dados = null;
                    respostaServico.Status = false;
                    respostaServico.Mensagem = "Email/Usuário já cadastrados!";
                    return respostaServico;
                }

                // Use PasswordHasher to generate the hashed password.
                var hashedPassword = PasswordHasher.HashPassword(hospedeRegistro.Senha);

                // Update HospedeModel to store the hashed password in the 'Senha' property.
                HospedeModel hospede = new HospedeModel()
                {
                    Nome = hospedeRegistro.Nome,
                    Email = hospedeRegistro.Email,
                    Senha = hashedPassword,
                    Telefone = hospedeRegistro.Telefone,
                    Nascimento = hospedeRegistro.Nascimento,
                    Cpf = hospedeRegistro.Cpf,
                    Endereco = hospedeRegistro.Endereco
                };

                _context.Add(hospede);
                await _context.SaveChangesAsync();

                respostaServico.Mensagem = "Usuário criado com sucesso!";
                respostaServico.Status = true;
                respostaServico.Dados = hospedeRegistro;
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : "Sem inner exception";
                respostaServico.Dados = null;
                respostaServico.Mensagem = $"{ex.Message} | Inner: {innerMessage}";
                respostaServico.Status = false;
            }

            return respostaServico;
        }

        public async Task<ResponseModel<string>> Login(HospedeLoginDto hospedeLogin)
        {
            ResponseModel<string> respostaServico = new ResponseModel<string>();

            try
            {
                var usuario = await _context.Hospedes.FirstOrDefaultAsync(u => u.Email == hospedeLogin.Email);
                if (usuario == null)
                {
                    respostaServico.Mensagem = "Credenciais inválidas!";
                    respostaServico.Status = false;
                    return respostaServico;
                }
                // Verify password using PasswordHasher's VerifyPassword method.
                if (!PasswordHasher.VerifyPassword(hospedeLogin.Senha, usuario.Senha))
                {
                    respostaServico.Mensagem = "Credenciais inválidas!";
                    respostaServico.Status = false;
                    return respostaServico;
                }

                // Create token using CriarToken method
                var token = CriarToken(usuario);
                respostaServico.Dados = token;
                respostaServico.Mensagem = "Usuário logado com sucesso!";
                respostaServico.Status = true;
            }
            catch (Exception ex)
            {
                respostaServico.Dados = null;
                respostaServico.Mensagem = ex.Message;
                respostaServico.Status = false;
            }

            return respostaServico;
        }

        public bool VerificaSeEmaileUsuarioJaExiste(HospedeCriacaoDto hospedeRegistro)
        {
            var usuario = _context.Hospedes.FirstOrDefault(u => u.Email == hospedeRegistro.Email || u.Nome == hospedeRegistro.Nome);
            return usuario == null;
        }

        public string CriarToken(HospedeModel hospede)
        {
            List<Claim> claims = new List<Claim>()
    {
        new Claim(ClaimTypes.Role, "hospede"),
        new Claim(ClaimTypes.Name, hospede.Nome),
        new Claim(ClaimTypes.Email, hospede.Email)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


    }
}
