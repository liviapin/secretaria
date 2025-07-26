using Secretaria.Aplicacao.Interfaces;
using Secretaria.DataTransfer;
using Secretaria.DataTransfer.Request;
using Secretaria.Dominio.Interfaces;
using Secretaria.Dominio.Models;

public class TurmaService : ITurmaService
{
    private readonly ITurmaRepository _turmaRepository;
    private readonly IMatriculaRepository _matriculaRepository;

    public TurmaService(ITurmaRepository turmaRepository, IMatriculaRepository matriculaRepository)
    {
        _turmaRepository = turmaRepository;
        _matriculaRepository = matriculaRepository;
    }

    public async Task<TurmaResponse> CriarTurmaAsync(CreateTurmaRequest turmaRequest)
    {
        var nome = turmaRequest.Nome?.Trim();
        var descricao = turmaRequest.Descricao?.Trim();

        await ValidarNomeDuplicadoAsync(nome);

        var turma = new Turma(nome, descricao);
        turma = await _turmaRepository.AdicionarAsync(turma);

        return MapearTurmaResponse(turma);
    }

    public async Task<TurmaResponse> AtualizarTurmaAsync(int id, UpdateTurmaRequest turmaRequest)
    {
        var turma = await _turmaRepository.ObterPorIdAsync(id);
        if (turma == null)
            throw new ArgumentException("Turma não encontrada");

        var nome = turmaRequest.Nome?.Trim();
        var descricao = turmaRequest.Descricao?.Trim();

        await ValidarNomeDuplicadoAsync(nome, id);

        turma.Atualizar(nome, descricao);

        var atualizado = await _turmaRepository.AtualizarAsync(turma);
        if (!atualizado)
            throw new ArgumentException("Erro ao atualizar turma");

        return MapearTurmaResponse(turma);
    }

    public async Task<PagedResponse<TurmaResponse>> ObterTurmasAsync(int pageNumber = 1, int pageSize = 10)
    {
        var turmas = await _turmaRepository.ObterTodosAsync(pageNumber, pageSize);
        var totalTurmas = await _turmaRepository.ContarTurmasAsync();

        var turmaResponses = new List<TurmaResponse>();

        foreach (var turma in turmas.OrderBy(t => t.Nome))
        {
            var alunos = await _matriculaRepository.ObterAlunosPorTurmaAsync(turma.Id, pageNumber, pageSize);
            turmaResponses.Add(new TurmaResponse
            {
                Id = turma.Id,
                Nome = turma.Nome,
                Descricao = turma.Descricao,
                NumeroAlunos = alunos.Count()
            });
        }

        return new PagedResponse<TurmaResponse>
        {
            Items = turmaResponses,
            TotalCount = totalTurmas
        };
    }

    public async Task<TurmaResponse> ObterTurmaPorIdAsync(int id)
    {
        var turma = await _turmaRepository.ObterPorIdAsync(id);
        if (turma == null)
            return null;

        var alunos = await _matriculaRepository.ContarAlunosPorTurmaAsync(turma.Id);

        return new TurmaResponse
        {
            Id = turma.Id,
            Nome = turma.Nome,
            Descricao = turma.Descricao,
            NumeroAlunos = alunos
        };
    }

    public async Task<bool> RemoverTurmaAsync(int id)
    {
        return await _turmaRepository.RemoverAsync(id);
    }

    private async Task ValidarNomeDuplicadoAsync(string nome, int? idAtual = null)
    {
        var turmaExistente = await _turmaRepository.ObterPorNomeAsync(nome);
        if (turmaExistente != null && turmaExistente.Id != idAtual)
            throw new InvalidOperationException("Já existe uma turma com esse nome.");
    }

    private TurmaResponse MapearTurmaResponse(Turma turma)
    {
        return new TurmaResponse
        {
            Id = turma.Id,
            Nome = turma.Nome,
            Descricao = turma.Descricao
        };
    }
}
