using Secretaria.Dominio.Models;

namespace Secretaria.Dominio.Interfaces
{
    public interface IAlunoRepository
    {
        Task<Aluno> AdicionarAsync(Aluno aluno);
        Task<Aluno> ObterPorIdAsync(int id);
        Task<IEnumerable<Aluno>> ObterTodosAsync(int pageNumber, int pageSize);
        Task<int> ContarAlunosAsync();
        Task<bool> AtualizarAsync(Aluno aluno);
        Task<bool> RemoverAsync(int id);
        Task<Aluno?> ObterPorEmailAsync(string email);
        Task<Aluno?> ObterPorCpfAsync(string cpf);
        Task<IEnumerable<Aluno>> ObterPorNomeAsync(string nome);
        Task<IEnumerable<Aluno>> ObterPorNomeAsync(string nome, int pageNumber, int pageSize);
        Task<int> ContarAlunosPorNomeAsync(string nome);
    }
}
