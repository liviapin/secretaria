using Secretaria.Dominio.Models;

namespace Secretaria.Dominio.Interfaces
{
    public interface ITurmaRepository
    {
        Task<Turma> AdicionarAsync(Turma turma);
        Task<Turma> ObterPorIdAsync(int id);
        Task<IEnumerable<Turma>> ObterTodosAsync();
        Task<bool> AtualizarAsync(Turma turma);
        Task<bool> RemoverAsync(int id);
    }
}
