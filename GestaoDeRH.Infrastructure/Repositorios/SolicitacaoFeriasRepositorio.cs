using GestaoDeRH.Dominio.Ferias;
using GestaoDeRH.Dominio.Interfaces;
using GestaoDeRH.Infra.BancoDeDados;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeRH.Infra.Repositorios
{
    public class SolicitacaoFeriasRepositorio : RepositorioGenerico<SolicitacaoFerias>, ISolicitacaoFeriasRepositorio
    {
        public SolicitacaoFeriasRepositorio(GestaoDeRhDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<SolicitacaoFerias>> ListarSolicitacoesPorColaborador(int colaboradorId)
        {
            return await _dbContext.Ferias
                                   .Where(x => x.Colaborador != null && x.ColaboradorId == colaboradorId)
                                   .ToListAsync();
        }

        public async Task<List<SolicitacaoFerias>> ListarSolicitacoesPorPeriodo(DateTime inicio, DateTime fim)
        {
            return await _dbContext.Ferias
                                   .Where(x => x.DataInicioFerias >= inicio && x.DataFimFerias <= fim)
                                   .ToListAsync();
        }

        public async Task<SolicitacaoFerias?> ObterSolicitacaoPorId(int id)
        {
            return await _dbContext.Ferias
                                   .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
