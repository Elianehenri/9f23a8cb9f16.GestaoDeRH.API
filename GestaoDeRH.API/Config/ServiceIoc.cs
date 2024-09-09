using GestaoDeRH.Aplicacao.Colaboradores;
using GestaoDeRH.Aplicacao.Colaboradores.Interfaces;
using GestaoDeRH.Aplicacao.ControlePonto;
using GestaoDeRH.Aplicacao.ControlePonto.Interfaces;
using GestaoDeRH.Aplicacao.Ferias;
using GestaoDeRH.Aplicacao.Ferias.Interfaces;
using GestaoDeRH.Aplicacao.FolhaDePagamento;
using GestaoDeRH.Aplicacao.FolhaDePagamento.Interfaces;

using GestaoDeRH.Aplicacao.Notificacoes;
using GestaoDeRH.Aplicacao.Recrutamento;
using GestaoDeRH.Aplicacao.Recrutamento.Interfaces;
using GestaoDeRH.Dominio.Ferias;


namespace GestaoDeRH.API.Config
{
    public class ServiceIoc
    {
        public static void RegisterServices(IServiceCollection builder)
        {

            builder.AddScoped<IMarcacaoPonto, MarcacaoPonto>();

            builder.AddScoped<IFecharFolhaDePagamento, FecharFolhaDePagamento>();

            builder.AddScoped<INotificarColaborador, NotificarColaborador>();

            builder.AddScoped<ICriarVaga, CriarVaga>();
            builder.AddScoped<INovoCandidato, NovoCandidato>();
            builder.AddScoped<IAprovarCandidato, AprovarCandidato>();
            builder.AddScoped<ISolicitacaoFerias, SolicitacaoFeriasServico>();
            builder.AddScoped<IColaboradorServico, ColaboradorServico>();

        }
    }
}
