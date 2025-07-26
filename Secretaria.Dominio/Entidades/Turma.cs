using Microsoft.EntityFrameworkCore;

namespace Secretaria.Dominio.Models
{
    [Index(nameof(Nome))]
    public class Turma
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }

        protected Turma() { }

        public Turma(string nome, string descricao)
        {
            ValidarNome(nome);
            ValidarDescricao(descricao);

            Nome = nome.Trim();
            Descricao = descricao.Trim();
        }

        public void Atualizar(string nome, string descricao)
        {
            ValidarNome(nome);
            ValidarDescricao(descricao);

            Nome = nome.Trim();
            Descricao = descricao.Trim();
        }

        private void ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser nulo, vazio ou espaços em branco.", nameof(nome));
            if (nome.Trim().Length < 3)
                throw new ArgumentException("Nome deve ter no mínimo 3 caracteres.", nameof(nome));
        }

        private void ValidarDescricao(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição não pode ser nula, vazia ou espaços em branco.", nameof(descricao));
            if (descricao.Trim().Length < 3)
                throw new ArgumentException("Descrição deve ter no mínimo 3 caracteres.", nameof(descricao));
        }
    }
}
