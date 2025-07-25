using Microsoft.EntityFrameworkCore;
using Secretaria.Dominio.Interfaces;
using Secretaria.Dominio.Models;
using Secretaria.Infra.Context;

namespace Secretaria.Infra.Repositories
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly SecretariaDbContext _context;

        public TurmaRepository(SecretariaDbContext context)
        {
            _context = context;
        }

        public async Task<Turma> AdicionarAsync(Turma turma)
        {
            _context.Turmas.Add(turma);
            await _context.SaveChangesAsync();
            return turma;
        }

        public async Task<IEnumerable<Turma>> ObterTodosAsync()
        {
            return await _context.Turmas.ToListAsync();
        }

        public async Task<Turma> ObterPorIdAsync(int id)
        {
            return await _context.Turmas.FindAsync(id);
        }

        public async Task<bool> AtualizarAsync(Turma turma)
        {
            _context.Turmas.Update(turma);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null)
                return false;
            _context.Turmas.Remove(turma);
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
