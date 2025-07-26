using Secretaria.Dominio.Models;

namespace Secretaria.Dominio.Interfaces
{
    public interface ITurmaRepository
    {
        Task<Turma> AdicionarAsync(Turma turma);
        Task<Turma> ObterPorIdAsync(int id);
        Task<IEnumerable<Turma>> ObterTodosAsync(int pageNumber = 1, int pageSize = 10);
        Task<int> ContarTurmasAsync();
        Task<bool> AtualizarAsync(Turma turma);
        Task<bool> RemoverAsync(int id);
        Task<Turma> ObterPorNomeAsync(string nome);
    }
}
