using FluentAssertions;
using Moq;
using Secretaria.Aplicacao.Interfaces;
using Secretaria.DataTransfer.Request;
using Secretaria.Dominio.Interfaces;
using Secretaria.Dominio.Models;

namespace Secretaria.Aplicacao.Tests.Services;

public class TurmaServiceTests
{
    private readonly Mock<ITurmaRepository> _turmaRepoMock = new();
    private readonly Mock<IMatriculaRepository> _matriculaRepoMock = new();
    private readonly ITurmaService _turmaService;

    public TurmaServiceTests()
    {
        _turmaService = new TurmaService(_turmaRepoMock.Object, _matriculaRepoMock.Object);
    }

    [Fact]
    public async Task Dado_CriarTurmaAsync_Deve_CriarTurmaComDadosValidos()
    {
        var request = new CreateTurmaRequest { Nome = "Matemática", Descricao = "Turma de Álgebra" };

        _turmaRepoMock.Setup(r => r.ObterPorNomeAsync("Matemática"))
            .ReturnsAsync((Turma?)null);

        _turmaRepoMock.Setup(r => r.AdicionarAsync(It.IsAny<Turma>()))
            .ReturnsAsync((Turma t) => t);

        var result = await _turmaService.CriarTurmaAsync(request);

        result.Nome.Should().Be("Matemática");
        result.Descricao.Should().Be("Turma de Álgebra");
    }

    [Fact]
    public async Task Dado_CriarTurmaAsync_Deve_FalharQuandoNomeDuplicado()
    {
        var request = new CreateTurmaRequest { Nome = "Matemática", Descricao = "Qualquer" };

        _turmaRepoMock.Setup(r => r.ObterPorNomeAsync("Matemática"))
            .ReturnsAsync(new Turma("Matemática", "Já existe"));

        Func<Task> act = async () => await _turmaService.CriarTurmaAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Já existe uma turma com esse nome.");
    }

    [Fact]
    public async Task Dado_CriarTurmaAsync_Deve_FalharQuandoNomeEhInvalido()
    {
        var request = new CreateTurmaRequest { Nome = " ", Descricao = "desc" };

        Func<Task> act = async () => await _turmaService.CriarTurmaAsync(request);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Nome não pode ser nulo, vazio ou espaços em branco.*");
    }

    [Fact]
    public async Task Dado_AtualizarTurmaAsync_Deve_AtualizarComSucesso()
    {
        var request = new UpdateTurmaRequest { Nome = "Atualizado", Descricao = "Nova desc" };
        var turma = new Turma("Antigo", "Antiga desc");

        _turmaRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(turma);
        _turmaRepoMock.Setup(r => r.ObterPorNomeAsync("Atualizado")).ReturnsAsync((Turma?)null);
        _turmaRepoMock.Setup(r => r.AtualizarAsync(turma)).ReturnsAsync(true);

        var result = await _turmaService.AtualizarTurmaAsync(1, request);


        result.Nome.Should().Be("Atualizado");
        result.Descricao.Should().Be("Nova desc");
    }

    [Fact]
    public async Task Dado_AtualizarTurmaAsync_Deve_FalharSeTurmaNaoExiste()
    {
        var request = new UpdateTurmaRequest { Nome = "Nova", Descricao = "Descricao" };
        _turmaRepoMock.Setup(r => r.ObterPorIdAsync(99)).ReturnsAsync((Turma?)null);


        Func<Task> act = async () => await _turmaService.AtualizarTurmaAsync(99, request);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Turma não encontrada");
    }

    [Fact]
    public async Task Dado_AtualizarTurmaAsync_Deve_FalharSeNomeDuplicado()
    {
        var request = new UpdateTurmaRequest { Nome = "Duplicado", Descricao = "Desc" };
        var turma = new Turma("Original", "Original desc");

        _turmaRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(turma);
        _turmaRepoMock.Setup(r => r.ObterPorNomeAsync("Duplicado"))
            .ReturnsAsync(new Turma("Duplicado", "Outra"));

        Func<Task> act = async () => await _turmaService.AtualizarTurmaAsync(1, request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Já existe uma turma com esse nome.");
    }
}
