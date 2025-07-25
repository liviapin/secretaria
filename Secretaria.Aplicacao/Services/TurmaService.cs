using Secretaria.Aplicacao.Interfaces;
using Secretaria.DataTransfer.Request;
using Secretaria.Dominio.Interfaces;
using Secretaria.Dominio.Models;

namespace Secretaria.Aplicacao.Services
{
    public class TurmaService : ITurmaService
    {
        private readonly ITurmaRepository _turmaRepository;
        private readonly IMatriculaRepository _matriculaRepository;

        public TurmaService(ITurmaRepository turmaRepository, IMatriculaRepository matriculaRepository)
        {
            _turmaRepository = turmaRepository;
            _matriculaRepository = matriculaRepository;
        }

        public async Task<TurmaResponse> CriarTurmaAsync(CreateTurmaRequest turmaRequest)
        {
            var turma = new Turma(turmaRequest.Nome, turmaRequest.Descricao);
            turma = await _turmaRepository.AdicionarAsync(turma);
            return new TurmaResponse
            {
                Id = turma.Id,
                Nome = turma.Nome,
                Descricao = turma.Descricao
            };
        }

        public async Task<IEnumerable<TurmaResponse>> ObterTurmasAsync()
        {
            var turmas = await _turmaRepository.ObterTodosAsync();
            var turmaResponses = new List<TurmaResponse>();

            foreach (var turma in turmas)
            {
                var alunos = await _matriculaRepository.ObterAlunosPorTurmaAsync(turma.Id);
                int numeroAlunos = alunos.Count();
                turmaResponses.Add(new TurmaResponse
                {
                    Id = turma.Id,
                    Nome = turma.Nome,
                    Descricao = turma.Descricao,
                    NumeroAlunos = numeroAlunos
                });
            }

            return turmaResponses;
        }

        public async Task<TurmaResponse> AtualizarTurmaAsync(int id, UpdateTurmaRequest turmaRequest)
        {
            var turma = await _turmaRepository.ObterPorIdAsync(id);
            if (turma == null)
                throw new ArgumentException("Turma não encontrada");

            turma.Atualizar(turmaRequest.Nome, turmaRequest.Descricao);

            var atualizado = await _turmaRepository.AtualizarAsync(turma);
            if (!atualizado)
                throw new ArgumentException("Erro ao atualizar turma");

            return new TurmaResponse
            {
                Id = turma.Id,
                Nome = turma.Nome,
                Descricao = turma.Descricao
            };
        }

        public async Task<bool> RemoverTurmaAsync(int id)
        {
            return await _turmaRepository.RemoverAsync(id);
        }
    }
}
