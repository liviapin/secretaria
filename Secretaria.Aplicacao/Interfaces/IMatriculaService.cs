using Secretaria.DataTransfer;
using Secretaria.DataTransfer.Request;
using Secretaria.DataTransfer.Response;

namespace Secretaria.Aplicacao.Interfaces
{
    public interface IMatriculaService
    {
        Task<MatriculaResponse> MatricularAsync(MatriculaRequest matriculaRequest);
        Task<PagedResponse<AlunoResponse>> ObterAlunosPorTurmaAsync(int turmaId, int pageNumber, int pageSize);
    }
}
