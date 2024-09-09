using GestaoDeRH.Aplicacao.Base;
using GestaoDeRH.Aplicacao.Ferias.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeRH.Aplicacao.Ferias.Interfaces
{
    public interface ISolicitacaoFerias
    {
        Task<ResultadoOperacao<SolicitacaoFeriasDto>> SolicitarFerias(SolicitacaoFeriasDto dto);
        Task<SolicitacaoFeriasDto> Obter(int id);
        Task<ResultadoOperacao<SolicitacaoFeriasDto>> Atualizar(SolicitacaoFeriasDto dto);
        Task<ResultadoOperacao<SolicitacaoFeriasDto>> Deletar(int id);
        Task<List<SolicitacaoFeriasDto>> Listar();
    }
}
