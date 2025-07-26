using System.ComponentModel.DataAnnotations;

namespace Secretaria.DataTransfer.Request
{
    public class UpdateTurmaRequest
    {
        [Required(ErrorMessage = "O nome da turma é obrigatório.")]
        [MinLength(3, ErrorMessage = "O nome da turma deve ter no mínimo 3 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição da turma é obrigatória.")]
        public string Descricao { get; set; }
    }
}
