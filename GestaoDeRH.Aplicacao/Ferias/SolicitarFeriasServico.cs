using GestaoDeRH.Aplicacao.Base;
using GestaoDeRH.Aplicacao.Ferias.DTO;
using GestaoDeRH.Aplicacao.Ferias.Interfaces;
using GestaoDeRH.Dominio.Ferias;
using GestaoDeRH.Dominio.Interfaces;
using GestaoDeRH.Dominio.Pessoas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeRH.Aplicacao.Ferias
{
    public  class SolicitarFeriasServico : ISolicitarFerias
    {
        private readonly IRepositorio<SolicitarFerias> _repositorioSolicitarFerias;
        private readonly IRepositorio<Colaborador> _repositorioColaboradores;


        public SolicitarFeriasServico(
            IRepositorio<SolicitarFerias> repositorioFerias,
            IRepositorio<Colaborador> repositorioColaboradores)
        {
            _repositorioSolicitarFerias = repositorioFerias;
            _repositorioColaboradores = repositorioColaboradores;
        }

        public async Task<ResultadoOperacao<SolicitarFeriasDto>> SolicitarFerias(SolicitarFeriasDto dto)
        {
            var resultado = new ResultadoOperacao<SolicitarFeriasDto>();

            var colaborador = await _repositorioColaboradores.Obter(dto.ColaboradorId);
            if (colaborador == null)
            {
                resultado.AdicionarErros(new List<string> { "Colaborador não encontrado." });
                return resultado;
            }

            var umAnoDeContrato = colaborador.DataInicioContratoDeTrabalho.AddYears(1);

            // Verifica se a data de início das férias é válida
            if (dto.DataInicioFerias < umAnoDeContrato && DateTime.Now < umAnoDeContrato)
            {
                resultado.AdicionarErros(new List<string> { $"Colaborador ainda não completou um ano de contrato. As férias só podem ser solicitadas a partir de {umAnoDeContrato.ToShortDateString()}." });
                return resultado;
            }

            var solicitacao = new SolicitarFerias
            {
                ColaboradorId = dto.ColaboradorId,
                DataInicioFerias = dto.DataInicioFerias,
                DataFimFerias = dto.DataFimFerias,
                DataSolicitacao = DateTime.Now
            };

            if (!solicitacao.EstaValida(out var erros))
            {
                resultado.AdicionarErros(erros);
                return resultado;
            }

            if (await VerificarSeSolicitacaoJaExiste(dto.ColaboradorId, dto.DataInicioFerias, dto.DataFimFerias))
            {
                resultado.AdicionarErros(new List<string> { "Já há uma solicitação neste período para este colaborador." });
                return resultado;
            }

            var totalDiasSolicitadosNoAno = await ObterTotalDiasSolicitadosNoAno(dto.ColaboradorId, dto.DataInicioFerias.Year);
            var diasSolicitados = dto.CalcularDiasSolicitados();
            if (totalDiasSolicitadosNoAno + diasSolicitados > 30)
            {
                resultado.AdicionarErros(new List<string> { $"Total de dias solicitados: {totalDiasSolicitadosNoAno + diasSolicitados} dias, total de dias disponíveis: 30 dias." });
                return resultado;
            }

            await _repositorioSolicitarFerias.Salvar(solicitacao);

            resultado.Dados = new SolicitarFeriasDto
            {
                ColaboradorId = solicitacao.ColaboradorId,
                DataInicioFerias = solicitacao.DataInicioFerias,
                DataFimFerias = solicitacao.DataFimFerias
            };
            return resultado;
        }
        public async Task<SolicitarFeriasDto> Obter(int id)
        {
            var solicitacao = await _repositorioSolicitarFerias.Obter(id);
            if (solicitacao == null) return null;

            return new SolicitarFeriasDto
            {
                ColaboradorId = solicitacao.ColaboradorId,
                DataInicioFerias = solicitacao.DataInicioFerias,
                DataFimFerias = solicitacao.DataFimFerias
            };
        }

        public async Task<ResultadoOperacao<SolicitarFeriasDto>> Atualizar(SolicitarFeriasDto dto)
        {
            var resultado = new ResultadoOperacao<SolicitarFeriasDto>();

            var solicitacaoExistente = await _repositorioSolicitarFerias.Obter(dto.ColaboradorId);
            if (solicitacaoExistente == null)
            {
                resultado.AdicionarErros(new List<string> { "Solicitação não encontrada." });
                return resultado;
            }

            solicitacaoExistente.ColaboradorId = dto.ColaboradorId;
            solicitacaoExistente.DataInicioFerias = dto.DataInicioFerias;
            solicitacaoExistente.DataFimFerias = dto.DataFimFerias;

            if (!solicitacaoExistente.EstaValida(out var erros))
            {
                resultado.AdicionarErros(erros);
                return resultado;
            }

            await _repositorioSolicitarFerias.Salvar(solicitacaoExistente);

            resultado.Dados = dto;
            return resultado;
        }

        public async Task<ResultadoOperacao<SolicitarFeriasDto>> Deletar(int id)
        {
            var resultado = new ResultadoOperacao<SolicitarFeriasDto>();

            try
            {
                var solicitacao = await _repositorioSolicitarFerias.Obter(id);
                if (solicitacao == null)
                {
                    resultado.AdicionarErros(new List<string> { "Solicitação de férias não encontrada." });
                    return resultado; // Sucesso será false automaticamente
                }

                await _repositorioSolicitarFerias.Deletar(id);
                // Não adicione erros para indicar sucesso
            }
            catch (Exception ex)
            {
                resultado.AdicionarErros(new List<string> { "Erro ao deletar solicitação de férias.", ex.Message });
            }

            return resultado; // Se Erros.Count == 0, Sucesso será true
        }

        public async Task<List<SolicitarFeriasDto>> Listar()
        {
            var solicitacoes = await _repositorioSolicitarFerias.Listar();
            return solicitacoes.Select(s => new SolicitarFeriasDto
            {
                Id = s.Id,
                ColaboradorId = s.ColaboradorId,
                DataInicioFerias = s.DataInicioFerias,
                DataFimFerias = s.DataFimFerias
            }).ToList();
        }

        public async Task<bool> VerificarSeSolicitacaoJaExiste(int colaboradorId, DateTime dataInicio, DateTime dataFim)
        {
            var solicitacoes = await _repositorioSolicitarFerias.Listar();

            return solicitacoes.Any(s => s.ColaboradorId == colaboradorId &&
                                         (s.DataInicioFerias <= dataFim && s.DataInicioFerias >= dataInicio ||
                                          s.DataFimFerias <= dataFim && s.DataFimFerias >= dataInicio ||
                                          s.DataInicioFerias <= dataInicio && s.DataFimFerias >= dataFim));
        }

        public async Task<int> ObterTotalDiasSolicitadosNoAno(int colaboradorId, int ano)
        {
            var solicitacoes = await _repositorioSolicitarFerias.Listar();
            return solicitacoes
                .Where(s => s.ColaboradorId == colaboradorId && s.DataInicioFerias.Year == ano)
                .Sum(s => (s.DataFimFerias - s.DataInicioFerias).Days + 1);
        }
    }
}
