using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secretaria.Aplicacao.Interfaces;
using Secretaria.DataTransfer;
using Secretaria.DataTransfer.Request;
using Secretaria.DataTransfer.Response;

namespace Secretaria.Api.Controllers
{
    [ApiController]
    //[Authorize(Roles = "Admin")]
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
                var matriculaResponse = await _matriculaService.MatricularAsync(matriculaRequest);
                return CreatedAtAction(nameof(ObterAlunosPorTurma), new { turmaId = matriculaResponse.TurmaId }, matriculaResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("turma/{turmaId}")]
        public async Task<ActionResult<PagedResponse<AlunoResponse>>> ObterAlunosPorTurma(int turmaId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var alunos = await _matriculaService.ObterAlunosPorTurmaAsync(turmaId, pageNumber, pageSize);
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
