using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Secretaria.Aplicacao.Services;
using Secretaria.DataTransfer.Request;
using Secretaria.Dominio.Interfaces;
using Secretaria.Dominio.Models;
using Secretaria.Infra.Context;

namespace Secretaria.Aplicacao.Tests.Services
{
    public class MatriculaServiceTests
    {
        private readonly Mock<IMatriculaRepository> _matriculaRepo = new();
        private readonly Mock<IAlunoRepository> _alunoRepo = new();
        private readonly Mock<ITurmaRepository> _turmaRepo = new();
        private readonly SecretariaDbContext _fakeContext;

        public MatriculaServiceTests()
        {
            var options = new DbContextOptionsBuilder<SecretariaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _fakeContext = new SecretariaDbContext(options);
        }

        [Fact]
        public async Task Dado_MatricularAsync_Deve_LancarExcecaoSeAlunoIdOuTurmaIdForemNulos()
        {
            var service = new MatriculaService(_matriculaRepo.Object, _alunoRepo.Object, _turmaRepo.Object, _fakeContext);
            var request = new MatriculaRequest { AlunoId = null, TurmaId = null };

            Func<Task> act = () => service.MatricularAsync(request);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("AlunoId é obrigatório.");
        }

        [Fact]
        public async Task Dado_MatricularAsync_Deve_LancarExcecaoSeAlunoJaEstiverMatriculado()
        {
            var alunoId = 1;
            var turmaId = 2;

            _matriculaRepo.Setup(r => r.ObterMatriculaAsync(alunoId, turmaId))
                .ReturnsAsync(new Matricula(alunoId, turmaId));

            var service = new MatriculaService(_matriculaRepo.Object, _alunoRepo.Object, _turmaRepo.Object, _fakeContext);
            var request = new MatriculaRequest { AlunoId = alunoId, TurmaId = turmaId };

            Func<Task> act = () => service.MatricularAsync(request);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Este aluno já está matriculado nesta turma.");
        }

        [Fact]
        public async Task Dado_MatricularAsync_Deve_LancarExcecaoSeAlunoNaoForEncontrado()
        {
            var alunoId = 1;
            var turmaId = 2;

            _matriculaRepo.Setup(r => r.ObterMatriculaAsync(alunoId, turmaId)).ReturnsAsync((Matricula?)null);
            _alunoRepo.Setup(r => r.ObterPorIdAsync(alunoId)).ReturnsAsync((Aluno?)null);

            var service = new MatriculaService(_matriculaRepo.Object, _alunoRepo.Object, _turmaRepo.Object, _fakeContext);
            var request = new MatriculaRequest { AlunoId = alunoId, TurmaId = turmaId };

            Func<Task> act = () => service.MatricularAsync(request);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Aluno não encontrado");
        }

        [Fact]
        public async Task Dado_MatricularAsync_Deve_LancarExcecaoSeTurmaNaoForEncontrada()
        {
            var alunoId = 1;
            var turmaId = 2;

            _matriculaRepo.Setup(r => r.ObterMatriculaAsync(alunoId, turmaId)).ReturnsAsync((Matricula?)null);
            _alunoRepo.Setup(r => r.ObterPorIdAsync(alunoId)).ReturnsAsync(new Aluno("João", DateTime.Parse("2000-01-01"), "12345678900", "joao@email.com", "Teste@123"));
            _turmaRepo.Setup(r => r.ObterPorIdAsync(turmaId)).ReturnsAsync((Turma?)null);

            var service = new MatriculaService(_matriculaRepo.Object, _alunoRepo.Object, _turmaRepo.Object, _fakeContext);
            var request = new MatriculaRequest { AlunoId = alunoId, TurmaId = turmaId };

            Func<Task> act = () => service.MatricularAsync(request);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Turma não encontrada");
        }

        [Fact]
        public async Task Dado_MatricularAsync_Deve_MatricularAlunoSeTodosOsDadosForemValidos()
        {
            var alunoId = 1;
            var turmaId = 2;
            var matriculaCriada = new Matricula(alunoId, turmaId) { };

            _matriculaRepo.Setup(r => r.ObterMatriculaAsync(alunoId, turmaId)).ReturnsAsync((Matricula?)null);
            _alunoRepo.Setup(r => r.ObterPorIdAsync(alunoId)).ReturnsAsync(new Aluno("João", DateTime.Parse("2000-01-01"), "12345678900", "joao@email.com", "Teste@123"));
            _turmaRepo.Setup(r => r.ObterPorIdAsync(turmaId)).ReturnsAsync(new Turma("Turma A", "Descricao"));
            _matriculaRepo.Setup(r => r.MatricularAsync(It.IsAny<Matricula>())).ReturnsAsync(matriculaCriada);

            var service = new MatriculaService(_matriculaRepo.Object, _alunoRepo.Object, _turmaRepo.Object, _fakeContext);
            var request = new MatriculaRequest { AlunoId = alunoId, TurmaId = turmaId };

            var result = await service.MatricularAsync(request);

            result.Should().NotBeNull();
            result.AlunoId.Should().Be(alunoId);
            result.TurmaId.Should().Be(turmaId);
        }
    }
}
