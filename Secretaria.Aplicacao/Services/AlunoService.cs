using Secretaria.Aplicacao.Interfaces;
using Secretaria.DataTransfer;
using Secretaria.DataTransfer.Request;
using Secretaria.DataTransfer.Response;
using Secretaria.Dominio.Interfaces;
using Secretaria.Dominio.Models;

public class AlunoService : IAlunoService
{
    private readonly IAlunoRepository _alunoRepository;

    public AlunoService(IAlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    public async Task<AlunoResponse> CriarAlunoAsync(CreateAlunoRequest req)
    {
        if (req is null)
            throw new ArgumentNullException(nameof(req));

        await ValidarDuplicidadeAsync(req.CPF, req.Email);

        var hashedPwd = BCrypt.Net.BCrypt.HashPassword(req.Senha);
        var aluno = new Aluno(req.Nome, req.DataNascimento, req.CPF, req.Email, hashedPwd);

        var alunoCriado = await _alunoRepository.AdicionarAsync(aluno);
        return MapearAlunoResponse(alunoCriado);
    }

    public async Task<AlunoResponse> AtualizarAlunoAsync(int id, UpdateAlunoRequest req)
    {
        var aluno = await _alunoRepository.ObterPorIdAsync(id)
            ?? throw new Exception("Aluno não encontrado");

        await ValidarDuplicidadeAsync(req.CPF, req.Email, id);

        if (!req.DataNascimento.HasValue)
            throw new ArgumentException("A data de nascimento é obrigatória na atualização.");

        if (req.DataNascimento.Value.Date >= DateTime.Today)
            throw new ArgumentException("A data de nascimento deve ser uma data passada.");

        aluno.Atualizar(req.Nome, req.DataNascimento.Value, req.CPF, req.Email);

        var atualizado = await _alunoRepository.AtualizarAsync(aluno);
        if (!atualizado)
            throw new Exception("Erro ao atualizar aluno");

        return MapearAlunoResponse(aluno);
    }
    private async Task ValidarDuplicidadeAsync(string cpf, string email, int? idAtual = null)
    {
        var cpfLimpo = RemoverFormatacaoCPF(cpf);

        var alunoCpf = await _alunoRepository.ObterPorCpfAsync(cpfLimpo);
        if (alunoCpf != null && alunoCpf.Id != idAtual)
            throw new InvalidOperationException("Já existe um aluno cadastrado com este Cpf.");

        var alunoEmail = await _alunoRepository.ObterPorEmailAsync(email);
        if (alunoEmail != null && alunoEmail.Id != idAtual)
            throw new InvalidOperationException("Já existe um aluno cadastrado com este e-mail.");
    }

    private string RemoverFormatacaoCPF(string cpf)
    {
        return new string(cpf.Where(char.IsDigit).ToArray());
    }

    private static AlunoResponse MapearAlunoResponse(Aluno aluno)
    {
        return new AlunoResponse
        {
            Id = aluno.Id,
            Nome = aluno.Nome,
            DataNascimento = aluno.DataNascimento,
            CPF = aluno.Cpf,
            Email = aluno.Email
        };
    }
    public async Task<PagedResponse<AlunoResponse>> ObterAlunosAsync(int pageNumber, int pageSize)
    {
        var alunos = await _alunoRepository.ObterTodosAsync(pageNumber, pageSize);
        var total = await _alunoRepository.ContarAlunosAsync();

        var itens = alunos
            .OrderBy(a => a.Nome)
            .Select(MapearAlunoResponse);

        return new PagedResponse<AlunoResponse> { Items = itens, TotalCount = total };
    }
    public async Task<AlunoResponse> ObterAlunoPorIdAsync(int id)
    {
        var aluno = await _alunoRepository.ObterPorIdAsync(id)
            ?? throw new Exception("Aluno não encontrado.");

        return MapearAlunoResponse(aluno);
    }
    public async Task<PagedResponse<AlunoResponse>> ObterPorNomeAsync(string nome, int pageNumber, int pageSize)
    {
        if (string.IsNullOrWhiteSpace(nome) || nome.Length < 3)
            throw new ArgumentException("O termo de busca deve ter ao menos 3 caracteres.");

        var alunos = await _alunoRepository.ObterPorNomeAsync(nome, pageNumber, pageSize);
        var total = await _alunoRepository.ContarAlunosPorNomeAsync(nome);

        var itens = alunos
            .OrderBy(a => a.Nome)
            .Select(MapearAlunoResponse);

        return new PagedResponse<AlunoResponse> { Items = itens, TotalCount = total };
    }
    public async Task<bool> RemoverAlunoAsync(int id)
    {
        return await _alunoRepository.RemoverAsync(id);
    }
}
