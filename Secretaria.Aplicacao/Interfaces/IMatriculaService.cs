using Secretaria.DataTransfer.Request;
using Secretaria.DataTransfer.Response;

namespace Secretaria.Aplicacao.Interfaces
{
    public interface IMatriculaService
    {
        Task<MatriculaResponse> MatrificarAsync(MatriculaRequest matriculaRequest);
        Task<IEnumerable<AlunoResponse>> ObterAlunosPorTurmaAsync(int turmaId);
    }
}
