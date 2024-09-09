using GestaoDeRH.Dominio.Base;
using GestaoDeRH.Dominio.Pessoas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeRH.Dominio.Ferias
{
    public class SolicitacaoFerias : Entidade
    {
        public int ColaboradorId { get; set; }
        public virtual Colaborador? Colaborador { get; set; }

        public DateTime DataInicioFerias { get; set; }
        public DateTime DataFimFerias { get; set; }

        public DateTime DataSolicitacao { get; set; }

        public SolicitacaoFerias()
        {
            DataSolicitacao = DateTime.Now;

        }

        // Método para validar a solicitação de férias
        public override bool EstaValida(out List<string> erros)
        {
            erros = new List<string>();

            // Verificar se o colaborador está presente
            if (ColaboradorId <= 0)
                erros.Add("ID do colaborador deve ser um valor positivo.");

            // Verificar se a data de início é anterior à data de fim
            if (DataInicioFerias > DataFimFerias)
                erros.Add("Data de início das férias não pode ser posterior à data de fim.");

            // Verificar a duração máxima permitida para as férias
            if ((DataFimFerias - DataInicioFerias).TotalDays > 30)
                erros.Add("A duração das férias não pode exceder 30 dias.");

            return erros.Count == 0;
        }
    }
}
