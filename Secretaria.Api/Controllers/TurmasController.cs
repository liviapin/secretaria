using Microsoft.AspNetCore.Mvc;
using Secretaria.Aplicacao.Interfaces;
using Secretaria.DataTransfer;
using Secretaria.DataTransfer.Request;

namespace Secretaria.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurmasController : ControllerBase
    {
        private readonly ITurmaService _turmaService;

        public TurmasController(ITurmaService turmaService)
        {
            _turmaService = turmaService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<TurmaResponse>>> ObterTodos(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var turmasPaginadas = await _turmaService.ObterTurmasAsync(pageNumber, pageSize);
                return Ok(turmasPaginadas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<TurmaResponse>> Criar([FromBody] CreateTurmaRequest turmaRequest)
        {
            try
            {
                var turmaCriada = await _turmaService.CriarTurmaAsync(turmaRequest);
                return CreatedAtAction(nameof(ObterPorId), new { id = turmaCriada.Id }, turmaCriada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TurmaResponse>> ObterPorId(int id)
        {
            try
            {
                var turma = await _turmaService.ObterTurmaPorIdAsync(id);

                if (turma == null)
                    return NotFound();

                return Ok(turma);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<TurmaResponse>> Atualizar(int id, [FromBody] UpdateTurmaRequest turmaRequest)
        {
            try
            {
                var turmaAtualizada = await _turmaService.AtualizarTurmaAsync(id, turmaRequest);
                return Ok(turmaAtualizada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var sucesso = await _turmaService.RemoverTurmaAsync(id);
                if (!sucesso)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
