using GestaoDeRH.Aplicacao.Ferias.DTO;
using GestaoDeRH.Aplicacao.Ferias.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeRH.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class SolicitarFeriasController : ControllerBase
    {
        private readonly ISolicitarFerias _solicitarFeriasService;

        public SolicitarFeriasController(ISolicitarFerias solicitarFeriasService)
        {
            _solicitarFeriasService = solicitarFeriasService;
        }


        [HttpGet("")]
        public async Task<ActionResult<List<SolicitarFeriasDto>>> ListarSolicitacoesFerias()
        {
            var solicitacoes = await _solicitarFeriasService.Listar();
            return Ok(solicitacoes);
        }

        [HttpPost("")]
        public async Task<IActionResult> CriarSolicitacaoFerias([FromBody] SolicitarFeriasDto novaSolicitacaoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _solicitarFeriasService.SolicitarFerias(novaSolicitacaoDto);

            if (!resultado.Sucesso)
            {
                return BadRequest(new { msg = string.Join("; ", resultado.Erros) });
            }

            return CreatedAtAction(nameof(ListarSolicitacoesFerias), new { id = resultado.Dados.ColaboradorId }, resultado.Dados);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarSolicitacaoFerias(int id, [FromBody] SolicitarFeriasDto solicitacaoDto)
        {
            if (id <= 0)
                return BadRequest("ID inválido.");

            solicitacaoDto.ColaboradorId = id; // Se o ID for passado como parte do DTO

            var resultado = await _solicitarFeriasService.Atualizar(solicitacaoDto);

            if (!resultado.Sucesso)
            {
                return BadRequest(new { msg = string.Join("; ", resultado.Erros) });
            }

            if (resultado.Dados == null)
            {
                return NotFound("Solicitação de férias não encontrada.");
            }

            return Ok(resultado.Dados);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarSolicitacaoFerias(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido.");

            var resultado = await _solicitarFeriasService.Deletar(id);

            if (!resultado.Sucesso)
            {
                return BadRequest(new { msg = string.Join("; ", resultado.Erros) });
            }

            // Retorna uma resposta com uma mensagem de sucesso
            return Ok(new { msg = "Solicitação de férias deletada com sucesso." });
        }
    }
}
