using Microsoft.EntityFrameworkCore;
using Secretaria.Aplicacao.Interfaces;
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

            if (string.IsNullOrWhiteSpace(nome) || nome.Length < 3)
                throw new ArgumentException("O nome do aluno deve ter no mínimo 3 caracteres.");

            if (string.IsNullOrWhiteSpace(cpf) || !Regex.IsMatch(cpf, @"^\d{11}$"))
                throw new ArgumentException("CPF inválido. Deve conter 11 dígitos.");

            if (string.IsNullOrWhiteSpace(email) ||
                !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Formato de e-mail inválido.");

            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("A senha não pode ser vazia.");

            const string senhaPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            if (!Regex.IsMatch(senha, senhaPattern))
                throw new ArgumentException(
                    "A senha deve ter no mínimo 8 caracteres, contendo letras maiúsculas, minúsculas, números e símbolos.");

            if (await _alunoRepository.ObterPorCpfAsync(cpf) != null)
                throw new InvalidOperationException("Já existe um aluno cadastrado com este CPF.");
            if (await _alunoRepository.ObterPorEmailAsync(email) != null)
                throw new InvalidOperationException("Já existe um aluno cadastrado com este e-mail.");

            var hashedPwd = BCrypt.Net.BCrypt.HashPassword(senha);

            var aluno = new Aluno(nome, req.DataNascimento, cpf, email, hashedPwd);

            try
            {
                aluno = await _alunoRepository.AdicionarAsync(aluno);
            }
            catch (DbUpdateException dbEx)
            {

                throw new InvalidOperationException(
                    "Falha ao criar aluno: já existe um registro com este CPF ou e-mail.", dbEx);
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

        public async Task<IEnumerable<AlunoResponse>> ObterAlunosAsync(int pageNumber, int pageSize)
        {
            var alunos = await _alunoRepository.ObterTodosAsync(pageNumber, pageSize);
            return alunos.Select(a => new AlunoResponse
            {
                Id = a.Id,
                Nome = a.Nome,
                DataNascimento = a.DataNascimento,
                CPF = a.CPF,
                Email = a.Email
            });
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

        public async Task<IEnumerable<AlunoResponse>> ObterPorNomeAsync(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome) || nome.Length < 3)
                throw new ArgumentException("O termo de busca deve ter ao menos 3 caracteres.");

            var alunos = await _alunoRepository.ObterPorNomeAsync(nome);
            return alunos.Select(a => new AlunoResponse
            {
                Id = a.Id,
                Nome = a.Nome,
                DataNascimento = a.DataNascimento,
                CPF = a.CPF,
                Email = a.Email
            });
        }

        public async Task<AlunoResponse> AtualizarAlunoAsync(int id, UpdateAlunoRequest alunoRequest)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(id);
            if (aluno == null)
                throw new Exception("Aluno não encontrado");

            aluno.Atualizar(
                alunoRequest.Nome,
                alunoRequest.DataNascimento,
                alunoRequest.CPF,
                alunoRequest.Email
            );

            var atualizado = await _alunoRepository.AtualizarAsync(aluno);
            if (!atualizado)
                throw new Exception("Erro ao atualizar aluno");

            return new AlunoResponse
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                DataNascimento = aluno.DataNascimento,
                CPF = aluno.CPF,
                Email = aluno.Email
            };
        }

        public async Task<bool> RemoverAlunoAsync(int id)
        {
            return await _alunoRepository.RemoverAsync(id);
        }
    }
}
