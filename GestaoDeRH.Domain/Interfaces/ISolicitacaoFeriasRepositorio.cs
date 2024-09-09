using GestaoDeRH.Dominio.Ferias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeRH.Dominio.Interfaces
{
    public interface ISolicitacaoFeriasRepositorio
    {
        Task<List<SolicitacaoFerias>> ListarSolicitacoesPorColaborador(int colaboradorId);
        Task<List<SolicitacaoFerias>> ListarSolicitacoesPorPeriodo(DateTime inicio, DateTime fim);
        Task<SolicitacaoFerias?> ObterSolicitacaoPorId(int id);
    }
}
