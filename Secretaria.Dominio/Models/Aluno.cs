namespace Secretaria.Dominio.Models
{
    public class Aluno
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string CPF { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }

        protected Aluno() { }

        public Aluno(string nome, DateTime dataNascimento, string cpf, string email, string senha)
        {
            Nome = nome;
            DataNascimento = dataNascimento;
            CPF = cpf;
            Email = email;
            Senha = senha;
        }

        public void Atualizar(string nome, DateTime dataNascimento, string cpf, string email)
        {
            Nome = nome;
            DataNascimento = dataNascimento;
            CPF = cpf;
            Email = email;
        }
    }
}
