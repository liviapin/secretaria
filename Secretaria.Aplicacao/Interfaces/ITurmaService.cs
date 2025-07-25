using Secretaria.DataTransfer.Request;
using Secretaria.DataTransfer.Response;

namespace Secretaria.Aplicacao.Interfaces
{
    public interface ITurmaService
    {
        Task<TurmaResponse> CriarTurmaAsync(CreateTurmaRequest turmaRequest);
        Task<IEnumerable<TurmaResponse>> ObterTurmasAsync();
        Task<TurmaResponse> AtualizarTurmaAsync(int id, UpdateTurmaRequest turmaRequest);
        Task<bool> RemoverTurmaAsync(int id);
    }
}
