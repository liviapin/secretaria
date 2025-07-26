using FluentAssertions;
using Secretaria.Dominio.Models;

namespace Secretaria.Dominio.Tests.Entidades
{
    public class TurmaTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("ab")]
        public void Dado_CriarTurmaComNomeInvalido_Deve_LancarExcecao(string nomeInvalido)
        {
            var descricao = "Descrição válida";

            Action act = () => new Turma(nomeInvalido, descricao);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Nome*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("ab")]
        public void Dado_CriarTurmaComDescricaoInvalida_Deve_LancarExcecao(string descricaoInvalida)
        {
            var nome = "Nome válido";

            Action act = () => new Turma(nome, descricaoInvalida);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Descrição*");
        }

        [Fact]
        public void Dado_CriarTurmaValida_Deve_CriarComSucesso()
        {
            var nome = "Nome válido";
            var descricao = "Descrição válida";

            var turma = new Turma(nome, descricao);

            turma.Nome.Should().Be(nome);
            turma.Descricao.Should().Be(descricao);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("ab")]
        public void AtualizarTurma_ComNomeInvalido_DeveLancarExcecao(string nomeInvalido)
        {
            var turma = new Turma("Nome Inicial", "Descrição Inicial");

            Action act = () => turma.Atualizar(nomeInvalido, "Descrição válida");

            act.Should().Throw<ArgumentException>()
                .WithMessage("Nome*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("ab")]
        public void Dado_AtualizarTurmaComDescricaoInvalida_Deve_LancarExcecao(string descricaoInvalida)
        {
            var turma = new Turma("Nome Inicial", "Descrição Inicial");

            Action act = () => turma.Atualizar("Nome válido", descricaoInvalida);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Descrição*");
        }

        [Fact]
        public void Dado_AtualizarTurmaValida_Deve_AtualizarComSucesso()
        {
            var turma = new Turma("Nome Inicial", "Descrição Inicial");
            var novoNome = "Novo Nome";
            var novaDescricao = "Nova Descrição";

            turma.Atualizar(novoNome, novaDescricao);

            turma.Nome.Should().Be(novoNome);
            turma.Descricao.Should().Be(novaDescricao);
        }
    }
}
