using Microsoft.EntityFrameworkCore;
using StayHub_BackEnd.Data;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.Services.Avaliacao
{
    public class AvaliacaoService : IAvaliacao
    {
        private readonly AppDbContext _context;
        public AvaliacaoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<AvaliacaoModel>> BuscarAvaliacao(int idAvaliacao)
        {
            ResponseModel<AvaliacaoModel> resposta = new ResponseModel<AvaliacaoModel>();

            try
            {
                var avaliacao = await _context.Avaliacoes.Include(h => h.Hospede).FirstOrDefaultAsync(x => x.Id == idAvaliacao); // .Include(q => q.Quarto)

                if (avaliacao == null)
                {
                    resposta.Mensagem = "Avaliacao não foi encontrada!";
                    return resposta;
                }

                resposta.Dados = avaliacao;
                resposta.Mensagem = "Avaliacao encontrada!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = $"Erro: {ex.Message}";
                if (ex.InnerException != null)
                {
                    resposta.Mensagem += $" InnerException: {ex.InnerException.Message}";
                }
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<AvaliacaoModel>>> BuscarAvaliacoesPorQuarto(int idQuarto)
        {
            ResponseModel<List<AvaliacaoModel>> resposta = new ResponseModel<List<AvaliacaoModel>>();

            try
            {
                var quarto = await _context.Quartos
                    .Include(q => q.Avaliacao)
                    .FirstOrDefaultAsync(q => q.Id == idQuarto);

                if (quarto == null)
                {
                    resposta.Mensagem = "Quarto não foi encontrado!";
                    return resposta;
                }

                resposta.Dados = quarto.Avaliacao.ToList();
                resposta.Mensagem = "Avaliações encontradas!";
                resposta.Status = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<AvaliacaoModel>>> CriarAvaliacao(AvaliacaoDto avaliacaoDto)
        {
            ResponseModel<List<AvaliacaoModel>> resposta = new ResponseModel<List<AvaliacaoModel>>();

            try
            {
                var avaliacao = new AvaliacaoModel()
                {
                    Descricao = avaliacaoDto.Descricao,
                    Avaliacao = avaliacaoDto.Avaliacao
                    //,Quarto = avaliacaoDto.Quarto,
                    //Hospede = avaliacaoDto.Hospede
                };

                _context.Add(avaliacao);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.Avaliacoes.ToListAsync();
                resposta.Mensagem = "Avaliacao criada com sucesso!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<AvaliacaoModel>> EditarAvaliacao(int idAvaliacao, AvaliacaoDto avaliacaoDto)
        {
            ResponseModel<AvaliacaoModel> resposta = new ResponseModel<AvaliacaoModel>();

            try
            {
                var avaliacao = await _context.Avaliacoes.FirstOrDefaultAsync(x => x.Id == idAvaliacao);

                if (avaliacao == null)
                {
                    resposta.Mensagem = "Avaliacao não foi encontrado!";
                    return resposta;
                }

                avaliacao.Descricao = avaliacaoDto.Descricao;
                avaliacao.Avaliacao = avaliacaoDto.Avaliacao;

                _context.Update(avaliacao);
                await _context.SaveChangesAsync();

                resposta.Dados = avaliacao;
                resposta.Mensagem = "Avaliacao atualizada com sucesso!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<AvaliacaoModel>>> ExcluirAvaliacao(int idAvaliacao)
        {
            ResponseModel<List<AvaliacaoModel>> resposta = new ResponseModel<List<AvaliacaoModel>>();

            try
            {
                var avaliacao = await _context.Avaliacoes.FirstOrDefaultAsync(x => x.Id == idAvaliacao);

                if (avaliacao == null)
                {
                    resposta.Mensagem = "avaliacao não foi encontrada!";
                    resposta.Status = false;
                    return resposta;
                }

                _context.Remove(avaliacao);
                await _context.SaveChangesAsync();

                resposta.Dados = await _context.Avaliacoes.ToListAsync();
                resposta.Mensagem = "avaliacao deletada com sucesso!";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<AvaliacaoModel>>> ListarAvaliacoes()
        {
            ResponseModel<List<AvaliacaoModel>> resposta = new ResponseModel<List<AvaliacaoModel>>();

            try
            {
                var avaliacao = await _context.Avaliacoes.Include(h => h.Hospede).ToListAsync(); //// .Include(q => q.Quarto)

                resposta.Dados = avaliacao;
                resposta.Mensagem = "Lista de avaliacoes Retornada!";

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
