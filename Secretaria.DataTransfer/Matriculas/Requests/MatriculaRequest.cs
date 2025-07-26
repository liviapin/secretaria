using System.ComponentModel.DataAnnotations;

namespace Secretaria.DataTransfer.Request
{
    public class MatriculaRequest
    {
        [Required(ErrorMessage = "Identificador do aluno é obrigatório.")]
        public int? AlunoId { get; set; }

        [Required(ErrorMessage = "Identificador da turma é obrigatório.")]
        public int? TurmaId { get; set; }
    }
}
