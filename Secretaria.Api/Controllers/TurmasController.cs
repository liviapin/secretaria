using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secretaria.Aplicacao.Interfaces;
using Secretaria.DataTransfer;
using Secretaria.DataTransfer.Request;

namespace Secretaria.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class TurmasController : ControllerBase
    {
        private readonly ITurmaService _turmaService;

        public TurmasController(ITurmaService turmaService)
        {
            _turmaService = turmaService;
        }

        /// <summary>
        /// Retorna uma lista paginada de turmas.
        /// </summary>
        /// <param name="pageNumber">Número da página (padrão: 1).</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10).</param>
        /// <returns>Lista paginada de turmas.</returns>
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

        /// <summary>
        /// Cria uma nova turma.
        /// </summary>
        /// <param name="request">Dados para criação da turma.</param>
        /// <returns>Turma criada.</returns>
        [HttpPost]
        public async Task<IActionResult> CriarTurma([FromBody] CreateTurmaRequest request)
        {
            var result = await _turmaService.CriarTurmaAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Atualiza uma turma existente.
        /// </summary>
        /// <param name="id">ID da turma a ser atualizada.</param>
        /// <param name="request">Dados para atualização da turma.</param>
        /// <returns>Turma atualizada.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] UpdateTurmaRequest request)
        {
            var result = await _turmaService.AtualizarTurmaAsync(id, request);
            return Ok(result);
        }

        /// <summary>
        /// Obtém uma turma pelo seu ID.
        /// </summary>
        /// <param name="id">ID da turma.</param>
        /// <returns>Turma encontrada ou NotFound se não existir.</returns>
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

        /// <summary>
        /// Remove uma turma pelo seu ID.
        /// </summary>
        /// <param name="id">ID da turma a ser removida.</param>
        /// <returns>NoContent se removida, NotFound se não existir.</returns>
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
