using Microsoft.AspNetCore.Mvc;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;
using StayHub_BackEnd.Services.Avaliacao;

namespace StayHub_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacaoController : Controller
    {
        private readonly IAvaliacao _iavaliacao;
        public AvaliacaoController(IAvaliacao iavaliacao)
        {
            _iavaliacao = iavaliacao;
        }

        [HttpGet("ListarAvaliacoes")]
        public async Task<ActionResult<ResponseModel<List<AvaliacaoModel>>>> ListarAvaliacoes()
        {
            var avaliacoes = await _iavaliacao.ListarAvaliacoes();
            return Ok(avaliacoes);
        }

        [HttpGet("BuscarAvaliacaoId/{idAvaliacao}")]
        public async Task<ActionResult<ResponseModel<AvaliacaoModel>>> BuscarAvaliacao(int idAvaliacao)
        {
            var avaliacao = await _iavaliacao.BuscarAvaliacao(idAvaliacao);
            return Ok(avaliacao);
        }

        [HttpGet("BuscarAvaliacaoPorQaurtoId/{idQuarto}")]
        public async Task<ActionResult<ResponseModel<AvaliacaoModel>>> BuscarAvaliacoesPorQuarto(int idQuarto)
        {
            var avaliacao = await _iavaliacao.BuscarAvaliacoesPorQuarto(idQuarto);
            return Ok(avaliacao);
        }

        [HttpPost("CriarAvaliacao")]
        public async Task<ActionResult<ResponseModel<AvaliacaoModel>>> CriarAvaliacao(AvaliacaoDto avaliacaoDto)
        {
            var avaliacao = await _iavaliacao.CriarAvaliacao(avaliacaoDto);
            return Ok(avaliacao);
        }

        [HttpPut("EditarAvaliacao/{idAvaliacao}")]
        public async Task<ActionResult<ResponseModel<AvaliacaoModel>>> EditarAvaliacao(int idAvaliacao, AvaliacaoDto avaliacaoDto)
        {
            var avaliacao = await _iavaliacao.EditarAvaliacao(idAvaliacao, avaliacaoDto);
            return Ok(avaliacao);
        }

        [HttpDelete("ExcluirAvaliacao/{idAvaliacao}")]
        public async Task<ActionResult<ResponseModel<AvaliacaoModel>>> ExcluirAvaliacao(int idAvaliacao)
        {
            var avaliacao = await _iavaliacao.ExcluirAvaliacao(idAvaliacao);
            return Ok(avaliacao);
        }

    }
}
