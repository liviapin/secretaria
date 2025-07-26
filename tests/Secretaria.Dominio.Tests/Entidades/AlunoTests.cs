using Secretaria.Dominio.Models;

namespace Secretaria.Dominio.Tests.Entidades;
public class AlunoTests
{
    [Fact]
    public void Dado_CriarAlunoComDadosValidos_Deve_CriarComSucesso()
    {
        var nome = "João Silva";
        var dataNascimento = new DateTime(2000, 1, 1);
        var cpf = "12345678901";
        var email = "joao@example.com";
        var senha = "senha123";

        var aluno = new Aluno(nome, dataNascimento, cpf, email, senha);

        Assert.Equal(nome, aluno.Nome);
        Assert.Equal(dataNascimento, aluno.DataNascimento);
        Assert.Equal(cpf, aluno.Cpf);
        Assert.Equal(email, aluno.Email);
        Assert.Equal(senha, aluno.Senha);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("Jo")]
    public void Dado_CriarAlunoNomeInvalido_Deve_LancarArgumentException(string nomeInvalido)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Aluno(nomeInvalido, DateTime.Today.AddYears(-20), "12345678901", "email@example.com", "senha"));

        Assert.Contains("nome", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dado_CriarAlunoCPFInvalido_Deve_LancarArgumentException()
    {
        var cpfInvalido = "123";

        var ex = Assert.Throws<ArgumentException>(() =>
            new Aluno("Nome", DateTime.Today.AddYears(-20), cpfInvalido, "email@example.com", "senha"));

        Assert.Contains("Cpf inválido", ex.Message);
    }

    [Theory]
    [InlineData("email_invalido")]
    [InlineData("email@")]
    [InlineData("email@.com")]
    public void Dado_CriarAlunoEmailInvalido_Deve_LancarArgumentException(string emailInvalido)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Aluno("Nome", DateTime.Today.AddYears(-20), "12345678901", emailInvalido, "senha"));

        Assert.Contains("e-mail", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dado_CriarAlunoDataNascimentoFutura_Deve_LancarArgumentException()
    {
        var dataFutura = DateTime.Today.AddDays(1);

        var ex = Assert.Throws<ArgumentException>(() =>
            new Aluno("Nome", dataFutura, "12345678901", "email@example.com", "senha"));

        Assert.Contains("data de nascimento", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dado_CriarAlunoSenhaVazia_Deve_LancarArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Aluno("Nome", DateTime.Today.AddYears(-20), "12345678901", "email@example.com", ""));

        Assert.Contains("senha", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dado_AtualizarAlunoComDadosValidos_Deve_AtualizarCorretamente()
    {
        var aluno = new Aluno("Antigo", DateTime.Today.AddYears(-20), "12345678901", "email@old.com", "senha");
        var novoNome = "Novo Nome";
        var novaData = DateTime.Today.AddYears(-25);
        var novoCpf = "09876543210";
        var novoEmail = "email@novo.com";

        aluno.Atualizar(novoNome, novaData, novoCpf, novoEmail);

        Assert.Equal(novoNome, aluno.Nome);
        Assert.Equal(novaData, aluno.DataNascimento);
        Assert.Equal(novoCpf, aluno.Cpf);
        Assert.Equal(novoEmail, aluno.Email);
    }

    [Fact]
    public void Dado_AtualizarAlunoComDadosInvalidos_Deve_LancarArgumentException()
    {
        var aluno = new Aluno("Nome", DateTime.Today.AddYears(-20), "12345678901", "email@example.com", "senha");

        var ex = Assert.Throws<ArgumentException>(() =>
            aluno.Atualizar("", DateTime.Today.AddYears(-20), "123", "emailinvalido"));

        Assert.Contains("nome", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}
