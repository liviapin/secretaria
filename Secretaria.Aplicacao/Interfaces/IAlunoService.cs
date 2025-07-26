using Secretaria.DataTransfer;
using Secretaria.DataTransfer.Request;
using Secretaria.DataTransfer.Response;

namespace Secretaria.Aplicacao.Interfaces
{
    public interface IAlunoService
    {
        Task<AlunoResponse> CriarAlunoAsync(CreateAlunoRequest alunoRequest);
        Task<PagedResponse<AlunoResponse>> ObterAlunosAsync(int pageNumber, int pageSize);
        Task<AlunoResponse> ObterAlunoPorIdAsync(int id);
        Task<AlunoResponse> AtualizarAlunoAsync(int id, UpdateAlunoRequest alunoRequest);
        Task<bool> RemoverAlunoAsync(int id);
        Task<PagedResponse<AlunoResponse>> ObterPorNomeAsync(string nome, int pageNumber, int pageSize);
    }
}
