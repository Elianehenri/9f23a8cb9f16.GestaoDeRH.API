using GestaoDeRH.Aplicacao.Base;
using GestaoDeRH.Aplicacao.Ferias.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeRH.Aplicacao.Ferias.Interfaces
{
    public interface ISolicitarFerias
    {
        Task<ResultadoOperacao<SolicitarFeriasDto>> SolicitarFerias(SolicitarFeriasDto dto);
        Task<SolicitarFeriasDto> Obter(int id);
        Task<ResultadoOperacao<SolicitarFeriasDto>> Atualizar(SolicitarFeriasDto dto);
        Task<ResultadoOperacao<SolicitarFeriasDto>> Deletar(int id);
        Task<List<SolicitarFeriasDto>> Listar();
    }
}
