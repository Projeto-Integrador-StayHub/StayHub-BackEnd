using Microsoft.EntityFrameworkCore;
using StayHub_BackEnd.Data;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace StayHub_BackEnd.Services.Quarto
{
    public class QuartoService : IQuarto
    {
        private readonly AppDbContext _context;
        private readonly ILogger<QuartoService> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuartoService(AppDbContext context, ILogger<QuartoService> logger, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ResponseModel<List<object>>> ListarQuartos()
        {
            ResponseModel<List<object>> resposta = new ResponseModel<List<object>>();

            try
            {
                _logger.LogInformation("Iniciando a consulta dos quartos...");

                var quartos = await _context.Quartos
                    .Include(q => q.Dono)
                    .Select(q => new
                    {
                        q.Id,
                        q.NomeQuarto,
                        q.Preco,
                        q.CapacidadePessoas,
                        q.Disponibilidade,
                        q.Comodidades,
                        Dono = new { q.Dono.Id, q.Dono.Nome }
                    })
                    .ToListAsync();

                _logger.LogInformation("Dados de quartos retornados:");
                _logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(quartos));

                resposta.Dados = quartos.Cast<object>().ToList();
                resposta.Mensagem = "Lista de quartos retornada!";
                resposta.Status = true;

                return resposta;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao listar quartos: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError($"InnerException: {ex.InnerException.Message}");
                }

                resposta.Mensagem = $"Erro ao listar quartos: {ex.Message}";
                resposta.Status = false;

                return resposta;
            }
        }

        public async Task<ResponseModel<QuartoModel>> BuscarQuarto(int idQuarto)
        {
            ResponseModel<QuartoModel> resposta = new ResponseModel<QuartoModel>();

            try
            {
                _logger.LogInformation($"Buscando quarto com ID: {idQuarto}");

                var quarto = await _context.Quartos
                    .Include(q => q.Dono)
                    .FirstOrDefaultAsync(q => q.Id == idQuarto);

                if (quarto == null)
                {
                    _logger.LogWarning($"Quarto com ID {idQuarto} não encontrado.");
                    resposta.Mensagem = "Quarto não encontrado!";
                    resposta.Status = false;
                    return resposta;
                }

                _logger.LogInformation($"Quarto encontrado: {quarto.NomeQuarto}");
                resposta.Dados = quarto;
                resposta.Mensagem = "Quarto encontrado!";
                resposta.Status = true;

                return resposta;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar quarto por ID: {ex.Message}");
                resposta.Mensagem = $"Erro ao buscar quarto por ID: {ex.Message}";
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<QuartoModel>> CriarQuarto(QuartoDto quartoDto)
        {
            ResponseModel<QuartoModel> resposta = new ResponseModel<QuartoModel>();

            try
            {
                _logger.LogInformation("Iniciando a criação de um novo quarto.");

                var quarto = new QuartoModel
                {
                    NomeQuarto = quartoDto.NomeQuarto,
                    Descricao = quartoDto.Descricao,
                    Preco = quartoDto.Preco,
                    CapacidadePessoas = quartoDto.CapacidadePessoas,
                    Disponibilidade = quartoDto.Disponibilidade,
                    Comodidades = quartoDto.Comodidades,
                    Estado = quartoDto.Estado,
                    Cidade = quartoDto.Cidade,
                    Endereco = quartoDto.Endereco,
                    DonoId = quartoDto.DonoId
                };

                if (quartoDto.Fotos != null && quartoDto.Fotos.Length > 0)
                {
                    // Nome do arquivo a partir do nome original
                    var fileName = Path.GetFileName(quartoDto.Fotos.FileName);

                    // Caminho para salvar o arquivo na pasta uploads
                    var uploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    // Verificar se o diretório existe, caso contrário, cria-lo
                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    // Caminho completo do arquivo
                    var filePath = Path.Combine(uploadsPath, fileName);

                    // Salvar o arquivo
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await quartoDto.Fotos.CopyToAsync(fileStream);
                    }

                    // Salvar o caminho relativo no DTO (path para a foto no servidor)
                    quartoDto.FotosPath = Path.Combine("uploads", fileName); // Caminho relativo para ser salvo no banco
                }

                _context.Quartos.Add(quarto);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Quarto {quarto.NomeQuarto} criado com sucesso.");

                resposta.Dados = quarto;
                resposta.Mensagem = "Quarto criado com sucesso!";
                resposta.Status = true;

                return resposta;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar quarto: {ex.Message}");
                resposta.Mensagem = $"Erro ao criar quarto: {ex.Message}";
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<QuartoModel>> EditarQuarto(int idQuarto, QuartoDto quartoDto)
        {
            ResponseModel<QuartoModel> resposta = new ResponseModel<QuartoModel>();

            try
            {
                _logger.LogInformation($"Iniciando a edição do quarto com ID: {idQuarto}");

                var quarto = await _context.Quartos
                    .FirstOrDefaultAsync(q => q.Id == idQuarto);

                if (quarto == null)
                {
                    _logger.LogWarning($"Quarto com ID {idQuarto} não encontrado.");
                    resposta.Mensagem = "Quarto não encontrado!";
                    resposta.Status = false;
                    return resposta;
                }

                quarto.NomeQuarto = quartoDto.NomeQuarto;
                quarto.Descricao = quartoDto.Descricao;
                quarto.Preco = quartoDto.Preco;
                quarto.CapacidadePessoas = quartoDto.CapacidadePessoas;
                quarto.Disponibilidade = quartoDto.Disponibilidade;
                quarto.Comodidades = quartoDto.Comodidades;
                quarto.Estado = quartoDto.Estado;
                quarto.Cidade = quartoDto.Cidade;
                quarto.Endereco = quartoDto.Endereco;

                if (quartoDto.Fotos != null && quartoDto.Fotos.Length > 0)
                {
                    var fileName = Path.GetFileName(quartoDto.Fotos.FileName);
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

                    // Salvar o arquivo na pasta uploads
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await quartoDto.Fotos.CopyToAsync(fileStream);
                    }

                    quartoDto.FotosPath = Path.Combine("uploads", fileName); // Salva o caminho no DTO
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Quarto com ID {idQuarto} atualizado com sucesso.");

                resposta.Dados = quarto;
                resposta.Mensagem = "Quarto atualizado com sucesso!";
                resposta.Status = true;

                return resposta;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao editar quarto: {ex.Message}");
                resposta.Mensagem = $"Erro ao editar quarto: {ex.Message}";
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<QuartoModel>> ExcluirQuarto(int idQuarto)
        {
            ResponseModel<QuartoModel> resposta = new ResponseModel<QuartoModel>();

            try
            {
                _logger.LogInformation($"Iniciando a exclusão do quarto com ID: {idQuarto}");

                var quarto = await _context.Quartos
                    .FirstOrDefaultAsync(q => q.Id == idQuarto);

                if (quarto == null)
                {
                    _logger.LogWarning($"Quarto com ID {idQuarto} não encontrado.");
                    resposta.Mensagem = "Quarto não encontrado!";
                    resposta.Status = false;
                    resposta.Dados = null;
                    return resposta;
                }

                var quartoExcluido = new QuartoModel
                {
                    Id = quarto.Id,
                    NomeQuarto = quarto.NomeQuarto,
                    Descricao = quarto.Descricao,
                    Preco = quarto.Preco,
                    CapacidadePessoas = quarto.CapacidadePessoas,
                    Disponibilidade = quarto.Disponibilidade,
                    Comodidades = quarto.Comodidades,
                    Estado = quarto.Estado,
                    Cidade = quarto.Cidade,
                    Endereco = quarto.Endereco,
                    DonoId = quarto.DonoId
                };

                _context.Quartos.Remove(quarto);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Quarto com ID {idQuarto} excluído com sucesso.");

                resposta.Mensagem = "Quarto excluído com sucesso!";
                resposta.Status = true;
                resposta.Dados = quartoExcluido;

                return resposta;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao excluir quarto: {ex.Message}");
                resposta.Mensagem = $"Erro ao excluir quarto: {ex.Message}";
                resposta.Status = false;
                resposta.Dados = null;
                return resposta;
            }
        }
    }
}
