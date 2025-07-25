namespace Secretaria.Dominio.Models
{
    public class Turma
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }

        protected Turma() { }
        public Turma(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;
        }

        public void Atualizar(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;
        }
    }
}
