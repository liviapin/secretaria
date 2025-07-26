using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Secretaria.Dominio.Models
{
    [Index(nameof(Nome))]
    public class Aluno
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }

        public Aluno(string nome, DateTime dataNascimento, string cpf, string email, string senha)
        {
            cpf = RemoverFormatacaoCPF(cpf);
            Validar(nome, dataNascimento, cpf, email, senha);

            Nome = nome.Trim();
            DataNascimento = dataNascimento;
            Cpf = cpf;
            Email = email.Trim();
            Senha = senha;
        }

        public void Atualizar(string nome, DateTime dataNascimento, string cpf, string email)
        {
            cpf = RemoverFormatacaoCPF(cpf);
            Validar(nome, dataNascimento, cpf, email);

            Nome = nome.Trim();
            DataNascimento = dataNascimento;
            Cpf = cpf;
            Email = email.Trim();
        }

        private string RemoverFormatacaoCPF(string cpf)
        {
            return new string(cpf.Where(char.IsDigit).ToArray());
        }


        private void Validar(string nome, DateTime dataNascimento, string cpf, string email, string senha = null)
        {
            if (string.IsNullOrWhiteSpace(nome) || nome.Trim().Length < 3)
                throw new ArgumentException("O nome do aluno deve ter no mínimo 3 caracteres.");

            if (!ValidarCPF(cpf))
                throw new ArgumentException("Cpf inválido. Deve conter 11 dígitos numéricos.");

            if (!ValidarEmail(email))
                throw new ArgumentException("Formato de e-mail inválido.");

            if (dataNascimento.Date >= DateTime.Today)
                throw new ArgumentException("A data de nascimento deve ser uma data passada.");

            if (senha != null && senha.Length == 0)
                throw new ArgumentException("A senha não pode ser vazia.");
        }

        private bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            var regex = new Regex(@"^\d{11}$");
            return regex.IsMatch(cpf.Trim());
        }

        private bool ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email.Trim());
        }
    }
}
