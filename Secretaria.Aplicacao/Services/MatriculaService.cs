using Microsoft.EntityFrameworkCore;
using Secretaria.Aplicacao.Interfaces;
using Secretaria.DataTransfer.Request;
using Secretaria.DataTransfer.Response;
using Secretaria.Dominio.Interfaces;
using Secretaria.Dominio.Models;
using Secretaria.Infra.Context;

namespace Secretaria.Aplicacao.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly ITurmaRepository _turmaRepository;
        private readonly SecretariaDbContext _context;

        public MatriculaService(
            IMatriculaRepository matriculaRepository,
            IAlunoRepository alunoRepository,
            ITurmaRepository turmaRepository,
            SecretariaDbContext context)
        {
            _matriculaRepository = matriculaRepository;
            _alunoRepository = alunoRepository;
            _turmaRepository = turmaRepository;
            _context = context;
        }

        public async Task<MatriculaResponse> MatrificarAsync(MatriculaRequest matriculaRequest)
        {
            var existingMatricula = await _matriculaRepository.ObterMatriculaAsync(matriculaRequest.AlunoId, matriculaRequest.TurmaId);
            if (existingMatricula != null)
            {
                throw new InvalidOperationException("Este aluno já está matriculado nesta turma.");
            }

            var aluno = await _alunoRepository.ObterPorIdAsync(matriculaRequest.AlunoId);
            if (aluno == null)
                throw new ArgumentException("Aluno não encontrado");

            var turma = await _turmaRepository.ObterPorIdAsync(matriculaRequest.TurmaId);
            if (turma == null)
                throw new ArgumentException("Turma não encontrada");

            var matricula = new Matricula(matriculaRequest.AlunoId, matriculaRequest.TurmaId);
            matricula = await _matriculaRepository.MatrificarAsync(matricula);

            return new MatriculaResponse
            {
                Id = matricula.Id,
                AlunoId = matricula.AlunoId,
                TurmaId = matricula.TurmaId
            };
        }

        public async Task<IEnumerable<AlunoResponse>> ObterAlunosPorTurmaAsync(int turmaId)
        {
            var alunos = await _matriculaRepository.ObterAlunosPorTurmaAsync(turmaId);
            return alunos.Select(a => new AlunoResponse
            {
                Id = a.Id,
                Nome = a.Nome,
                DataNascimento = a.DataNascimento,
                CPF = a.CPF,
                Email = a.Email
            });
        }

        public async Task<int> ContarAlunosPorTurmaAsync(int turmaId)
        {
            return await _context.Matriculas
                .Where(m => m.TurmaId == turmaId)
                .CountAsync();
        }

        public async Task<Matricula?> ObterMatriculaAsync(int alunoId, int turmaId)
        {
            return await _context.Matriculas
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.TurmaId == turmaId);
        }
    }
}
