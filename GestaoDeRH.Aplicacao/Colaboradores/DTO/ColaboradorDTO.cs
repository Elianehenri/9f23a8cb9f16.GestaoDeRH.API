using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeRH.Aplicacao.Colaboradores.DTO
{
    public class ColaboradorDTO
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public DateTime DataDeNascimento { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public DateTime DataInicioContratoDeTrabalho { get; set; }
        public DateTime? DataFimContratoDeTrabalho { get; set; }
        public decimal Salario { get; set; }
        public string Cargo { get; set; }
    }
}
