using Microsoft.EntityFrameworkCore;
using Secretaria.Dominio.Interfaces;
using Secretaria.Dominio.Models;
using Secretaria.Infra.Context;

namespace Secretaria.Infra.Repositories
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly SecretariaDbContext _context;

        public MatriculaRepository(SecretariaDbContext context)
        {
            _context = context;
        }

        public async Task<Matricula> MatricularAsync(Matricula matricula)
        {
            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();
            return matricula;
        }

        public async Task<IEnumerable<Aluno>> ObterAlunosPorTurmaAsync(int turmaId, int pageNumber, int pageSize)
        {
            return await _context.Matriculas
                .Include(m => m.Aluno)
                .Where(m => m.TurmaId == turmaId)
                .OrderBy(m => m.Aluno.Nome)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(m => m.Aluno)
                .ToListAsync();
        }

        public async Task<int> ContarAlunosPorTurmaAsync(int turmaId)
        {
            return await _context.Matriculas
                .Where(m => m.TurmaId == turmaId)
                .Select(m => m.AlunoId)
                .Distinct()
                .CountAsync();
        }

        public async Task<Matricula?> ObterMatriculaAsync(int alunoId, int turmaId)
        {
            return await _context.Matriculas
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.TurmaId == turmaId);
        }
    }
}
