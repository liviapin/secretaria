namespace Secretaria.Dominio.Models
{
    public class Matricula
    {
        public int Id { get; private set; }
        public int AlunoId { get; private set; }
        public int TurmaId { get; private set; }
        public Aluno Aluno { get; private set; }
        public Turma Turma { get; private set; }

        protected Matricula() { }

        public Matricula(int alunoId, int turmaId)
        {
            if (alunoId <= 0)
                throw new ArgumentException("AlunoId é obrigatório e deve ser maior que zero.");

            if (turmaId <= 0)
                throw new ArgumentException("TurmaId é obrigatório e deve ser maior que zero.");

            AlunoId = alunoId;
            TurmaId = turmaId;
        }
    }
}
