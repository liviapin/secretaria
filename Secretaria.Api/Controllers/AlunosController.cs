using Microsoft.AspNetCore.Mvc;
using Secretaria.Aplicacao.Interfaces;
using Secretaria.DataTransfer.Request;
using Secretaria.DataTransfer.Response;

namespace Secretaria.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoService _alunoService;

        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlunoResponse>>> ObterTodos(int pageNumber = 1, int pageSize = 10)
        {
            var alunos = await _alunoService.ObterAlunosAsync(pageNumber, pageSize);
            return Ok(alunos);
        }

        [HttpPost]
        public async Task<ActionResult<AlunoResponse>> Criar([FromBody] CreateAlunoRequest alunoRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var alunoCriado = await _alunoService.CriarAlunoAsync(alunoRequest);
            return CreatedAtAction(nameof(ObterPorId), new { id = alunoCriado.Id }, alunoCriado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlunoResponse>> ObterPorId(int id)
        {
            var aluno = await _alunoService.ObterAlunoPorIdAsync(id);
            return Ok(aluno);
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<AlunoResponse>>> BuscarPorNome(string nome)
        {
            var alunos = await _alunoService.ObterPorNomeAsync(nome);
            return Ok(alunos);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AlunoResponse>> Atualizar(int id, [FromBody] UpdateAlunoRequest alunoRequest)
        {
            var alunoAtualizado = await _alunoService.AtualizarAlunoAsync(id, alunoRequest);
            return Ok(alunoAtualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var sucesso = await _alunoService.RemoverAlunoAsync(id);
            if (!sucesso)
                return NotFound();

            return NoContent();
        }
    }
}
