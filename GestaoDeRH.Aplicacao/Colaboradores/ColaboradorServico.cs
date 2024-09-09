using GestaoDeRH.Aplicacao.Colaboradores.DTO;
using GestaoDeRH.Aplicacao.Colaboradores.Interfaces;
using GestaoDeRH.Dominio.Interfaces;
using GestaoDeRH.Dominio.Pessoas;
using GestaoDeRH.Dominio.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeRH.Aplicacao.Colaboradores
{
    public class ColaboradorServico: IColaboradorServico
    {
        private readonly IRepositorio<Colaborador> _repositorioColaborador;
        private const int IdadeMinima = 14;

        public ColaboradorServico(IRepositorio<Colaborador> repositorioColaborador)
        {
            _repositorioColaborador = repositorioColaborador;
        }

        public async Task<List<ColaboradorDTO>> ListarColaboradores()
        {
            var colaboradores = await _repositorioColaborador.Listar();
            return colaboradores.Select(c => new ColaboradorDTO
            {
                Nome = c.Nome,
                Sobrenome = c.Sobrenome,
                Email = c.Email,
                DataDeNascimento = c.DataDeNascimento,
                CPF = c.CPF,
                RG = c.RG,
                DataInicioContratoDeTrabalho = c.DataInicioContratoDeTrabalho,
                DataFimContratoDeTrabalho = c.DataFimContratoDeTrabalho,
                Salario = c.Salario,
                Cargo = c.Cargo
            }).ToList();
        }

        public async Task<ColaboradorDTO> CriarColaborador(ColaboradorDTO colaboradorDto)
        {
            // Validação de email
            if (!ValidacaoEmail.IsValidEmail(colaboradorDto.Email))
            {
                throw new ArgumentException("O email informado não é válido.");
            }

            if (await EmailJaCadastrado(colaboradorDto.Email))
            {
                throw new ArgumentException("O email já está cadastrado para outro colaborador.");
            }

            // Validação de CPF
            if (!ValidacaoCpf.IsValidCpf(colaboradorDto.CPF))
            {
                throw new ArgumentException("O CPF informado não é válido.");
            }

            if (await CpfJaCadastrado(colaboradorDto.CPF))
            {
                throw new ArgumentException("O CPF já está cadastrado para outro colaborador.");
            }

            // Validação de idade mínima (14 anos)
            if (!ValidacaoIdade.IsIdadeValida(colaboradorDto.DataDeNascimento, IdadeMinima))
            {
                throw new ArgumentException("O colaborador deve ter pelo menos 14 anos de idade.");
            }
            var colaborador = new Colaborador
            {
                Nome = colaboradorDto.Nome,
                Sobrenome = colaboradorDto.Sobrenome,
                Email = colaboradorDto.Email,
                DataDeNascimento = colaboradorDto.DataDeNascimento,
                CPF = colaboradorDto.CPF,
                RG = colaboradorDto.RG,
                DataInicioContratoDeTrabalho = colaboradorDto.DataInicioContratoDeTrabalho,
                DataFimContratoDeTrabalho = colaboradorDto.DataFimContratoDeTrabalho,
                Salario = colaboradorDto.Salario,
                Cargo = colaboradorDto.Cargo
            };

            colaborador = await _repositorioColaborador.Salvar(colaborador);

            return new ColaboradorDTO
            {
                Nome = colaborador.Nome,
                Sobrenome = colaborador.Sobrenome,
                Email = colaborador.Email,
                DataDeNascimento = colaborador.DataDeNascimento,
                CPF = colaborador.CPF,
                RG = colaborador.RG,
                DataInicioContratoDeTrabalho = colaborador.DataInicioContratoDeTrabalho,
                DataFimContratoDeTrabalho = colaborador.DataFimContratoDeTrabalho,
                Salario = colaborador.Salario,
                Cargo = colaborador.Cargo
            };
        }

        public async Task<ColaboradorDTO> AtualizarOuCriarColaborador(ColaboradorDTO colaboradorDto)
        {
            var colaborador = new Colaborador
            {
                Nome = colaboradorDto.Nome,
                Sobrenome = colaboradorDto.Sobrenome,
                Email = colaboradorDto.Email,
                DataDeNascimento = colaboradorDto.DataDeNascimento,
                CPF = colaboradorDto.CPF,
                RG = colaboradorDto.RG,
                DataInicioContratoDeTrabalho = colaboradorDto.DataInicioContratoDeTrabalho,
                DataFimContratoDeTrabalho = colaboradorDto.DataFimContratoDeTrabalho,
                Salario = colaboradorDto.Salario,
                Cargo = colaboradorDto.Cargo
            };

            colaborador = await _repositorioColaborador.Salvar(colaborador);

            return new ColaboradorDTO
            {
                Nome = colaborador.Nome,
                Sobrenome = colaborador.Sobrenome,
                Email = colaborador.Email,
                DataDeNascimento = colaborador.DataDeNascimento,
                CPF = colaborador.CPF,
                RG = colaborador.RG,
                DataInicioContratoDeTrabalho = colaborador.DataInicioContratoDeTrabalho,
                DataFimContratoDeTrabalho = colaborador.DataFimContratoDeTrabalho,
                Salario = colaborador.Salario,
                Cargo = colaborador.Cargo
            };
        }

        public async Task DeletarColaborador(int id)
        {
            await _repositorioColaborador.Deletar(id);
        }

        private async Task<bool> EmailJaCadastrado(string email)
        {
            var colaboradores = await _repositorioColaborador.Listar();
            return colaboradores.Any(c => c.Email == email);
        }
        private async Task<bool> CpfJaCadastrado(string cpf)
        {
            var colaboradores = await _repositorioColaborador.Listar();
            return colaboradores.Any(c => c.CPF == cpf);
        }
    }
}
