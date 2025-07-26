using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secretaria.Aplicacao.Interfaces;
using Secretaria.DataTransfer;
using Secretaria.DataTransfer.Request;
using Secretaria.DataTransfer.Response;

namespace Secretaria.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoService _alunoService;

        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        /// <summary>
        /// Retorna uma lista paginada de alunos.
        /// </summary>
        /// <param name="pageNumber">Número da página (padrão 1).</param>
        /// <param name="pageSize">Tamanho da página (padrão 10).</param>
        /// <returns>Lista paginada de alunos.</returns>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<AlunoResponse>>> ObterTodos(int pageNumber = 1, int pageSize = 10)
        {
            var alunosPaged = await _alunoService.ObterAlunosAsync(pageNumber, pageSize);
            return Ok(alunosPaged);
        }

        /// <summary>
        /// Cria um novo aluno.
        /// </summary>
        /// <param name="alunoRequest">Dados para criação do aluno.</param>
        /// <returns>Aluno criado.</returns>
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

        /// <summary>
        /// Obtém um aluno pelo seu ID.
        /// </summary>
        /// <param name="id">ID do aluno.</param>
        /// <returns>Aluno encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AlunoResponse>> ObterPorId(int id)
        {
            var aluno = await _alunoService.ObterAlunoPorIdAsync(id);
            return Ok(aluno);
        }

        /// <summary>
        /// Busca alunos pelo nome com paginação.
        /// </summary>
        /// <param name="nome">Nome ou parte do nome para busca.</param>
        /// <param name="pageNumber">Número da página (padrão 1).</param>
        /// <param name="pageSize">Tamanho da página (padrão 10).</param>
        /// <returns>Lista paginada de alunos que correspondem ao nome.</returns>
        [HttpGet("buscar")]
        public async Task<ActionResult<PagedResponse<AlunoResponse>>> BuscarPorNome(string nome, int pageNumber = 1, int pageSize = 10)
        {
            var alunosPaged = await _alunoService.ObterPorNomeAsync(nome, pageNumber, pageSize);
            return Ok(alunosPaged);
        }

        /// <summary>
        /// Atualiza os dados de um aluno existente.
        /// </summary>
        /// <param name="id">ID do aluno a ser atualizado.</param>
        /// <param name="alunoRequest">Novos dados do aluno.</param>
        /// <returns>Aluno atualizado.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<AlunoResponse>> Atualizar(int id, [FromBody] UpdateAlunoRequest alunoRequest)
        {
            var alunoAtualizado = await _alunoService.AtualizarAlunoAsync(id, alunoRequest);
            return Ok(alunoAtualizado);
        }

        /// <summary>
        /// Remove um aluno pelo seu ID.
        /// </summary>
        /// <param name="id">ID do aluno a ser removido.</param>
        /// <returns>Retorna NoContent se removido com sucesso ou NotFound se não existir.</returns>
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
