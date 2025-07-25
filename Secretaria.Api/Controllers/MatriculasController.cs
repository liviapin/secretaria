using Microsoft.AspNetCore.Mvc;
using Secretaria.Aplicacao.Interfaces;
using Secretaria.DataTransfer.Request;
using Secretaria.DataTransfer.Response;

namespace Secretaria.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatriculasController : ControllerBase
    {
        private readonly IMatriculaService _matriculaService;

        public MatriculasController(IMatriculaService matriculaService)
        {
            _matriculaService = matriculaService;
        }

        [HttpPost]
        public async Task<ActionResult<MatriculaResponse>> Matricular([FromBody] MatriculaRequest matriculaRequest)
        {
            try
            {
                var matriculaResponse = await _matriculaService.MatrificarAsync(matriculaRequest);
                return CreatedAtAction(nameof(ObterAlunosPorTurma), new { turmaId = matriculaResponse.TurmaId }, matriculaResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("turma/{turmaId}")]
        public async Task<ActionResult<IEnumerable<AlunoResponse>>> ObterAlunosPorTurma(int turmaId)
        {
            try
            {
                var alunos = await _matriculaService.ObterAlunosPorTurmaAsync(turmaId);
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
