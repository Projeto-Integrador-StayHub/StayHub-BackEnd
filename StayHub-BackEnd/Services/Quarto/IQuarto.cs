using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.Services.Quarto
{
    public interface IQuarto
    {
        Task<ResponseModel<List<QuartoModel>>> ListarQuartos();
        Task<ResponseModel<QuartoModel>> BuscarQuarto(int idQuarto);
        Task<ResponseModel<List<QuartoModel>>> CriarQuarto(QuartoDto quartoDto);
        Task<ResponseModel<QuartoModel>> EditarQuarto(int idQuarto, QuartoDto quartoDto);
        Task<ResponseModel<List<QuartoModel>>> ExcluirQuarto(int idQuarto);
    }
}
