using FluentAssertions;
using Secretaria.Dominio.Models;

namespace Secretaria.Dominio.Tests.Entidades
{
    public class MatriculaTests
    {
        [Fact]
        public void Dadio_Construtor_Deve_CriarMatriculaQuandoIdsValidos()
        {
            int alunoId = 1;
            int turmaId = 2;

            var matricula = new Matricula(alunoId, turmaId);

            matricula.AlunoId.Should().Be(alunoId);
            matricula.TurmaId.Should().Be(turmaId);
        }

        [Theory]
        [InlineData(0, 1, "AlunoId é obrigatório e deve ser maior que zero.")]
        [InlineData(-5, 2, "AlunoId é obrigatório e deve ser maior que zero.")]
        [InlineData(1, 0, "TurmaId é obrigatório e deve ser maior que zero.")]
        [InlineData(2, -3, "TurmaId é obrigatório e deve ser maior que zero.")]
        public void Dado_Construtor_Deve_LancarExcecaoQuandoIdsInvalidos(int alunoId, int turmaId, string expectedMessage)
        {
            Action act = () => new Matricula(alunoId, turmaId);

            act.Should()
               .Throw<ArgumentException>()
               .WithMessage(expectedMessage);
        }
    }
}
