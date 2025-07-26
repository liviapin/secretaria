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
            try
            {
                var alunosPaged = await _alunoService.ObterAlunosAsync(pageNumber, pageSize);
                return Ok(alunosPaged);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter alunos: {ex.Message}");
            }
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
                return BadRequest(ModelState);

            try
            {
                var alunoCriado = await _alunoService.CriarAlunoAsync(alunoRequest);
                return CreatedAtAction(nameof(ObterPorId), new { id = alunoCriado.Id }, alunoCriado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar aluno: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém um aluno pelo seu ID.
        /// </summary>
        /// <param name="id">ID do aluno.</param>
        /// <returns>Aluno encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AlunoResponse>> ObterPorId(int id)
        {
            try
            {
                var aluno = await _alunoService.ObterAlunoPorIdAsync(id);
                if (aluno == null)
                    return NotFound();

                return Ok(aluno);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter aluno: {ex.Message}");
            }
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
            try
            {
                var alunosPaged = await _alunoService.ObterPorNomeAsync(nome, pageNumber, pageSize);
                return Ok(alunosPaged);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar alunos: {ex.Message}");
            }
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
            try
            {
                var alunoAtualizado = await _alunoService.AtualizarAlunoAsync(id, alunoRequest);
                return Ok(alunoAtualizado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar aluno: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove um aluno pelo seu ID.
        /// </summary>
        /// <param name="id">ID do aluno a ser removido.</param>
        /// <returns>Retorna NoContent se removido com sucesso ou NotFound se não existir.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var sucesso = await _alunoService.RemoverAlunoAsync(id);
                if (!sucesso)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao remover aluno: {ex.Message}");
            }
        }
    }
}
