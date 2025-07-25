using Secretaria.Dominio.Models;

namespace Secretaria.Dominio.Interfaces
{
    public interface IMatriculaRepository
    {
        Task<Matricula> MatrificarAsync(Matricula matricula);
        Task<IEnumerable<Aluno>> ObterAlunosPorTurmaAsync(int turmaId);
        Task<int> ContarAlunosPorTurmaAsync(int turmaId);
        Task<Matricula?> ObterMatriculaAsync(int alunoId, int turmaId);
    }
}
