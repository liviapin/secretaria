using System.ComponentModel.DataAnnotations;

public class CreateTurmaRequest
{
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [MinLength(3, ErrorMessage = "O nome da turma deve ter no mínimo 3 caracteres.")]
    public string Nome { get; set; }

    public string Descricao { get; set; }
}
