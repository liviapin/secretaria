using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Secretaria.Dominio.Models;

namespace Secretaria.Infra.Context
{
    public class SecretariaDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public SecretariaDbContext(DbContextOptions<SecretariaDbContext> options)
            : base(options) { }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Turma> Turmas { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
    }
}
