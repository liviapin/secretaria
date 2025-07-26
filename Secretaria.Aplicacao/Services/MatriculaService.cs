using Microsoft.EntityFrameworkCore;
using Secretaria.Aplicacao.Interfaces;
using Secretaria.DataTransfer;
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

        public async Task<MatriculaResponse> MatricularAsync(MatriculaRequest matriculaRequest)
        {
            if (!matriculaRequest.AlunoId.HasValue || !matriculaRequest.TurmaId.HasValue)
                throw new ArgumentException("AlunoId e TurmaId são obrigatórios.");

            var alunoId = matriculaRequest.AlunoId.Value;
            var turmaId = matriculaRequest.TurmaId.Value;

            if (await _matriculaRepository.ObterMatriculaAsync(alunoId, turmaId) != null)
                throw new InvalidOperationException("Este aluno já está matriculado nesta turma.");

            var aluno = await _alunoRepository.ObterPorIdAsync(alunoId)
                ?? throw new ArgumentException("Aluno não encontrado");

            var turma = await _turmaRepository.ObterPorIdAsync(turmaId)
                ?? throw new ArgumentException("Turma não encontrada");

            var matricula = new Matricula(alunoId, turmaId);
            matricula = await _matriculaRepository.MatricularAsync(matricula);

            return new MatriculaResponse
            {
                Id = matricula.Id,
                AlunoId = matricula.AlunoId,
                TurmaId = matricula.TurmaId
            };
        }


        public async Task<PagedResponse<AlunoResponse>> ObterAlunosPorTurmaAsync(int turmaId, int pageNumber, int pageSize)
        {
            var alunos = await _matriculaRepository.ObterAlunosPorTurmaAsync(turmaId, pageNumber, pageSize);

            var totalAlunos = await _matriculaRepository.ContarAlunosPorTurmaAsync(turmaId);

            var alunosResponse = alunos
                .Select(a => new AlunoResponse
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    DataNascimento = a.DataNascimento,
                    CPF = a.CPF,
                    Email = a.Email
                }).ToList();

            return new PagedResponse<AlunoResponse>
            {
                Items = alunosResponse,
                TotalCount = totalAlunos
            };
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
