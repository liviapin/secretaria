using Secretaria.DataTransfer;
using Secretaria.DataTransfer.Request;

namespace Secretaria.Aplicacao.Interfaces
{
    public interface ITurmaService
    {
        Task<TurmaResponse> CriarTurmaAsync(CreateTurmaRequest turmaRequest);
        Task<PagedResponse<TurmaResponse>> ObterTurmasAsync(int pageNumber = 1, int pageSize = 10);
        Task<TurmaResponse> ObterTurmaPorIdAsync(int id);
        Task<TurmaResponse> AtualizarTurmaAsync(int id, UpdateTurmaRequest turmaRequest);
        Task<bool> RemoverTurmaAsync(int id);
    }
}
