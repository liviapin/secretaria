using Secretaria.Dominio.Models;

namespace Secretaria.Dominio.Interfaces
{
    public interface IMatriculaRepository
    {
        Task<Matricula> MatricularAsync(Matricula matricula);
        Task<IEnumerable<Aluno>> ObterAlunosPorTurmaAsync(int turmaId, int PageNumber, int pageSize);
        Task<int> ContarAlunosPorTurmaAsync(int turmaId);
        Task<Matricula?> ObterMatriculaAsync(int alunoId, int turmaId);
    }
}
