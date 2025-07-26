using Moq;
using Secretaria.DataTransfer.Request;
using Secretaria.Dominio.Interfaces;
using Secretaria.Dominio.Models;

namespace Secretaria.Aplicacao.Tests.Services;
public class AlunoServiceTests
{
    private readonly Mock<IAlunoRepository> _repoMock;
    private readonly AlunoService _service;

    public AlunoServiceTests()
    {
        _repoMock = new Mock<IAlunoRepository>();
        _service = new AlunoService(_repoMock.Object);
    }

    [Fact]
    public async Task Dado_CriarAlunoAsyncComDadosValidos_Deve_CriarAluno()
    {
        var req = new CreateAlunoRequest
        {
            Nome = "João Silva",
            DataNascimento = new DateTime(2000, 1, 1),
            CPF = "12345678901",
            Email = "joao@example.com",
            Senha = "senha123"
        };

        _repoMock.Setup(r => r.ObterPorCpfAsync(It.IsAny<string>()))
            .ReturnsAsync((Aluno)null);
        _repoMock.Setup(r => r.ObterPorEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Aluno)null);
        _repoMock.Setup(r => r.AdicionarAsync(It.IsAny<Aluno>()))
            .ReturnsAsync((Aluno a) => a);

        var result = await _service.CriarAlunoAsync(req);

        Assert.NotNull(result);
        Assert.Equal(req.Nome, result.Nome);
        Assert.Equal(req.CPF, result.CPF);
        Assert.Equal(req.Email, result.Email);
        _repoMock.Verify(r => r.AdicionarAsync(It.IsAny<Aluno>()), Times.Once);
    }

    [Fact]
    public async Task Dado_CriarAlunoAsyncCpfDuplicado_Deve_LancarExcecao()
    {
        var req = new CreateAlunoRequest
        {
            Nome = "João Silva",
            DataNascimento = new DateTime(2000, 1, 1),
            CPF = "12345678901",
            Email = "joao@example.com",
            Senha = "senha123"
        };

        _repoMock.Setup(r => r.ObterPorCpfAsync(It.IsAny<string>()))
            .ReturnsAsync(new Aluno("Outro", DateTime.Today.AddYears(-30), req.CPF, "outro@email.com", "senha"));

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarAlunoAsync(req));
        Assert.Contains("Cpf", ex.Message);
    }

    [Fact]
    public async Task Dado_CriarAlunoAsyncEmailDuplicado_Deve_LancarExcecao()
    {
        var req = new CreateAlunoRequest
        {
            Nome = "João Silva",
            DataNascimento = new DateTime(2000, 1, 1),
            CPF = "12345678901",
            Email = "joao@example.com",
            Senha = "senha123"
        };

        _repoMock.Setup(r => r.ObterPorCpfAsync(It.IsAny<string>()))
            .ReturnsAsync((Aluno)null);
        _repoMock.Setup(r => r.ObterPorEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new Aluno("Outro", DateTime.Today.AddYears(-30), "99999999999", req.Email, "senha"));

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CriarAlunoAsync(req));
        Assert.Contains("e-mail", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Dado_AtualizarAlunoAsyncAlunoNaoEncontrado_Deve_LancarExcecao()
    {
        _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((Aluno)null);

        var req = new UpdateAlunoRequest
        {
            Nome = "Nome",
            DataNascimento = DateTime.Today.AddYears(-20),
            CPF = "12345678901",
            Email = "email@example.com"
        };

        var ex = await Assert.ThrowsAsync<Exception>(() => _service.AtualizarAlunoAsync(1, req));
        Assert.Contains("não encontrado", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Dado_AtualizarAlunoAsyncCpfDuplicado_Deve_LancarExcecao()
    {
        var alunoExistente = new Aluno("Antigo", DateTime.Today.AddYears(-30), "12345678901", "antigo@email.com", "senha");
        var alunoDuplicadoCpf = new Aluno("Outro", DateTime.Today.AddYears(-25), "09876543210", "outro@email.com", "senha");

        var req = new UpdateAlunoRequest
        {
            Nome = "Novo Nome",
            DataNascimento = DateTime.Today.AddYears(-20),
            CPF = alunoDuplicadoCpf.Cpf,
            Email = "novo@email.com"
        };

        _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync(alunoExistente);
        _repoMock.Setup(r => r.ObterPorCpfAsync(req.CPF)).ReturnsAsync(alunoDuplicadoCpf);
        _repoMock.Setup(r => r.ObterPorEmailAsync(It.IsAny<string>())).ReturnsAsync((Aluno)null);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AtualizarAlunoAsync(1, req));
        Assert.Contains("Cpf", ex.Message);
    }

    [Fact]
    public async Task Dado_RemoverAlunoAsync_Deve_ChamarRepositorio()
    {
        _repoMock.Setup(r => r.RemoverAsync(It.IsAny<int>())).ReturnsAsync(true);

        var result = await _service.RemoverAlunoAsync(1);

        Assert.True(result);
        _repoMock.Verify(r => r.RemoverAsync(1), Times.Once);
    }

    [Fact]
    public async Task Dado_ObterAlunoPorIdAsyncAlunoExistente_Deve_RetornarAluno()
    {
        var aluno = new Aluno("Nome", DateTime.Today.AddYears(-20), "12345678901", "email@example.com", "senha");
        _repoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(aluno);

        var result = await _service.ObterAlunoPorIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(aluno.Nome, result.Nome);
    }
}
