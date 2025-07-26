using Microsoft.EntityFrameworkCore;
using Secretaria.Dominio.Interfaces;
using Secretaria.Dominio.Models;
using Secretaria.Infra.Context;

namespace Secretaria.Infra.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly SecretariaDbContext _context;

        public AlunoRepository(SecretariaDbContext context)
        {
            _context = context;
        }

        public async Task<Aluno> AdicionarAsync(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
            return aluno;
        }

        public async Task<Aluno> ObterPorIdAsync(int id)
        {
            return await _context.Alunos.FindAsync(id);
        }

        public async Task<IEnumerable<Aluno>> ObterTodosAsync(int pageNumber, int pageSize)
        {
            return await _context.Alunos
                .OrderBy(a => a.Nome)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> ContarAlunosAsync()
        {
            return await _context.Alunos.CountAsync();
        }

        public async Task<IEnumerable<Aluno>> ObterPorNomeAsync(string nome, int pageNumber, int pageSize)
        {
            return await _context.Alunos
                .Where(a => a.Nome.Contains(nome))
                .OrderBy(a => a.Nome)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Aluno>> ObterPorNomeAsync(string nome)
        {
            return await _context.Alunos
                .Where(a => a.Nome.Contains(nome))
                .OrderBy(a => a.Nome)
                .ToListAsync();
        }

        public async Task<int> ContarAlunosPorNomeAsync(string nome)
        {
            return await _context.Alunos
                .Where(a => a.Nome.Contains(nome))
                .CountAsync();
        }

        public async Task<bool> AtualizarAsync(Aluno aluno)
        {
            _context.Alunos.Update(aluno);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
                return false;
            _context.Alunos.Remove(aluno);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Aluno?> ObterPorEmailAsync(string email)
        {
            return await _context.Alunos
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Aluno?> ObterPorCpfAsync(string cpf)
        {
            return await _context.Alunos
                .FirstOrDefaultAsync(a => a.CPF == cpf);
        }
    }
}
