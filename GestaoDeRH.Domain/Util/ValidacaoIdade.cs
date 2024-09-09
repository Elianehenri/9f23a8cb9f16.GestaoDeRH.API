using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeRH.Dominio.Util
{
    public static class ValidacaoIdade
    {
        public static bool IsIdadeValida(DateTime dataDeNascimento, int idadeMinima)
        {
            var idade = CalcularIdade(dataDeNascimento);
            return idade >= idadeMinima;
        }

        private static int CalcularIdade(DateTime dataDeNascimento)
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - dataDeNascimento.Year;

            // Ajusta a idade se o aniversário ainda não ocorreu neste ano
            if (dataDeNascimento.Date > hoje.AddYears(-idade)) idade--;

            return idade;
        }

    }
}
