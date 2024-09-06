using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeRH.Aplicacao.Ferias.DTO
{
    public  class SolicitarFeriasDto
    {
        public int Id { get; set; }
        public int ColaboradorId { get; set; }
        public DateTime DataInicioFerias { get; set; }
        public DateTime DataFimFerias { get; set; }

        // Método para validar a duração das férias
        public bool ValidarDuracaoFerias()
        {
            return (DataFimFerias - DataInicioFerias).TotalDays <= 30;
        }

        // Método para calcular o total de dias solicitados
        public int CalcularDiasSolicitados()
        {
            return (DataFimFerias - DataInicioFerias).Days + 1;
        }
    }
}
