using GestaoDeRH.Aplicacao.Colaboradores.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeRH.Aplicacao.Colaboradores.Interfaces
{
    public interface IColaboradorServico
    {
        Task<List<ColaboradorDTO>> ListarColaboradores();
        Task<ColaboradorDTO> CriarColaborador(ColaboradorDTO colaboradorDto);
        Task<ColaboradorDTO> AtualizarOuCriarColaborador(ColaboradorDTO colaboradorDto);
        Task DeletarColaborador(int id);
    }
}
