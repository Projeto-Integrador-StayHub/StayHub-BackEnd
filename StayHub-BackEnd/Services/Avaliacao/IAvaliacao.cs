using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.Services.Avaliacao
{
    public interface IAvaliacao
    {
        Task<ResponseModel<List<AvaliacaoModel>>> ListarAvaliacoes();
        Task<ResponseModel<AvaliacaoModel>> BuscarAvaliacao(int idAvaliacao);
        Task<ResponseModel<List<AvaliacaoModel>>> BuscarAvaliacoesPorQuarto(int idQuarto);
        Task<ResponseModel<List<AvaliacaoModel>>> CriarAvaliacao(AvaliacaoDto avaliacaoDto);
        Task<ResponseModel<AvaliacaoModel>> EditarAvaliacao(int idAvaliacao, AvaliacaoDto avaliacaoDto);
        Task<ResponseModel<List<AvaliacaoModel>>> ExcluirAvaliacao(int idAvaliacao);
    }
}
