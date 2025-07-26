using Microsoft.EntityFrameworkCore;
using Secretaria.Aplicacao.Interfaces;
using Secretaria.DataTransfer;
using Secretaria.DataTransfer.Request;
using Secretaria.DataTransfer.Response;
using Secretaria.Dominio.Interfaces;
using Secretaria.Dominio.Models;
using System.Text.RegularExpressions;

namespace Secretaria.Aplicacao.Services
{
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

            var nome = req.Nome?.Trim();
            var cpf = req.CPF?.Trim();
            var email = req.Email?.Trim();
            var senha = req.Senha;
            var dataNascimento = req.DataNascimento.Value;

            ValidarDadosAluno(nome, cpf, email, dataNascimento);
            await ValidarDuplicidadeAsync(cpf, email);
            ValidarSenha(senha);

            var hashedPwd = BCrypt.Net.BCrypt.HashPassword(senha);
            var aluno = new Aluno(nome, dataNascimento, cpf, email, hashedPwd);

            try
            {
                aluno = await _alunoRepository.AdicionarAsync(aluno);
            }
            catch (DbUpdateException dbEx)
            {
                throw new InvalidOperationException(
                    "Falha ao criar aluno: já existe um registro com este CPF ou e-mail.", dbEx);
            }

            return MapearAlunoResponse(aluno);
        }

        public async Task<AlunoResponse> AtualizarAlunoAsync(int id, UpdateAlunoRequest alunoRequest)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(id);
            if (aluno == null)
                throw new Exception("Aluno não encontrado");

            var nome = alunoRequest.Nome?.Trim();
            var cpf = alunoRequest.CPF?.Trim();
            var email = alunoRequest.Email?.Trim();
            var dataNascimento = alunoRequest.DataNascimento;

            ValidarDadosAluno(nome, cpf, email, dataNascimento);
            await ValidarDuplicidadeAsync(cpf, email, id);

            aluno.Atualizar(nome, alunoRequest.DataNascimento, cpf, email);

            var atualizado = await _alunoRepository.AtualizarAsync(aluno);
            if (!atualizado)
                throw new Exception("Erro ao atualizar aluno");

            return MapearAlunoResponse(aluno);
        }


        private void ValidarDadosAluno(string nome, string cpf, string email, DateTime? dataNascimento = null)
        {
            if (string.IsNullOrWhiteSpace(nome) || nome.Length < 3)
                throw new ArgumentException("O nome do aluno deve ter no mínimo 3 caracteres.");

            if (string.IsNullOrWhiteSpace(cpf) || !Regex.IsMatch(cpf, @"^\d{11}$"))
                throw new ArgumentException("CPF inválido. Deve conter 11 dígitos.");

            if (string.IsNullOrWhiteSpace(email) ||
                !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Formato de e-mail inválido.");

            if (!dataNascimento.HasValue)
                throw new ArgumentException("A data de nascimento é obrigatória.");

            if (dataNascimento.Value.Date >= DateTime.Today)
                throw new ArgumentException("A data de nascimento deve ser uma data passada.");

        }

        private void ValidarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("A senha não pode ser vazia.");

            const string senhaPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            if (!Regex.IsMatch(senha, senhaPattern))
                throw new ArgumentException(
                    "A senha deve ter no mínimo 8 caracteres, contendo letras maiúsculas, minúsculas, números e símbolos.");
        }

        private async Task ValidarDuplicidadeAsync(string cpf, string email, int? idAtual = null)
        {
            var alunoCpf = await _alunoRepository.ObterPorCpfAsync(cpf);
            if (alunoCpf != null && alunoCpf.Id != idAtual)
                throw new InvalidOperationException("Já existe um aluno cadastrado com este CPF.");

            var alunoEmail = await _alunoRepository.ObterPorEmailAsync(email);
            if (alunoEmail != null && alunoEmail.Id != idAtual)
                throw new InvalidOperationException("Já existe um aluno cadastrado com este e-mail.");
        }

        private AlunoResponse MapearAlunoResponse(Aluno aluno)
        {
            return new AlunoResponse
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                DataNascimento = aluno.DataNascimento,
                CPF = aluno.CPF,
                Email = aluno.Email
            };
        }

        public async Task<PagedResponse<AlunoResponse>> ObterAlunosAsync(int pageNumber, int pageSize)
        {
            var alunos = await _alunoRepository.ObterTodosAsync(pageNumber, pageSize);

            var totalAlunos = await _alunoRepository.ContarAlunosAsync();

            var alunosResponse = alunos
                .OrderBy(a => a.Nome)
                .Select(a => new AlunoResponse
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    DataNascimento = a.DataNascimento,
                    CPF = a.CPF,
                    Email = a.Email
                });

            return new PagedResponse<AlunoResponse>
            {
                Items = alunosResponse,
                TotalCount = totalAlunos
            };
        }

        public async Task<AlunoResponse> ObterAlunoPorIdAsync(int id)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(id);
            if (aluno == null)
            {
                throw new Exception("Aluno não encontrado.");
            }

            return new AlunoResponse
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                DataNascimento = aluno.DataNascimento,
                CPF = aluno.CPF,
                Email = aluno.Email
            };
        }

        public async Task<PagedResponse<AlunoResponse>> ObterPorNomeAsync(string nome, int pageNumber, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(nome) || nome.Length < 3)
                throw new ArgumentException("O termo de busca deve ter ao menos 3 caracteres.");

            var alunos = await _alunoRepository.ObterPorNomeAsync(nome, pageNumber, pageSize);

            var totalAlunos = await _alunoRepository.ContarAlunosPorNomeAsync(nome);

            var alunosResponse = alunos
                .OrderBy(a => a.Nome)
                .Select(a => new AlunoResponse
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    DataNascimento = a.DataNascimento,
                    CPF = a.CPF,
                    Email = a.Email
                });

            return new PagedResponse<AlunoResponse>
            {
                Items = alunosResponse,
                TotalCount = totalAlunos
            };
        }

        public async Task<bool> RemoverAlunoAsync(int id)
        {
            return await _alunoRepository.RemoverAsync(id);
        }
    }
}
