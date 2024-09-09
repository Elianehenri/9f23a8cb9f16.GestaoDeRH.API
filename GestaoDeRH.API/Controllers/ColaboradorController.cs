using GestaoDeRH.Aplicacao.Colaboradores.DTO;
using GestaoDeRH.Aplicacao.Colaboradores.Interfaces;
using GestaoDeRH.Dominio.Interfaces;
using GestaoDeRH.Dominio.Pessoas;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeRH.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
   
    public class ColaboradorController : ControllerBase
    {
        private readonly IColaboradorServico _colaboradorServico;

        public ColaboradorController(IColaboradorServico colaboradorService)
        {
            _colaboradorServico = colaboradorService;
        }

        [HttpGet("")]
        public async Task<List<ColaboradorDTO>> ListarColaboradores()
        {
            return await _colaboradorServico.ListarColaboradores();
        }

        [HttpPost("")]
        public async Task<IActionResult> CriarColaborador(ColaboradorDTO novoColaboradorDto)
        {
            try
            {
                var colaborador = await _colaboradorServico.CriarColaborador(novoColaboradorDto);
                return Ok(colaborador);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("")]
        public async Task<IActionResult> AtualizarOuCriarColaborador(ColaboradorDTO novoColaboradorDto)
        {
            var colaborador = await _colaboradorServico.AtualizarOuCriarColaborador(novoColaboradorDto);
            return Ok(colaborador);
        }

        [HttpDelete("")]
        public async Task<IActionResult> DeletarColaborador(int id)
        {
            await _colaboradorServico.DeletarColaborador(id);
            return NoContent();
        }
    }
}
