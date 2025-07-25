namespace Secretaria.DataTransfer.Request
{
    public class UpdateAlunoRequest
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
    }
}
