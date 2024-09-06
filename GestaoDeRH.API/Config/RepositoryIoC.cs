using GestaoDeRH.Dominio.ControlePonto;
using GestaoDeRH.Dominio.Interfaces;

using GestaoDeRH.Infra.Repositorios;


namespace GestaoDeRH.API.Config
{
    public class RepositoryIoC
    {
        public static void RegisterRepositories(IServiceCollection builder)
        {
            builder.AddScoped(typeof(IRepositorio<>), typeof(RepositorioGenerico<>));
            builder.AddScoped<IPontoRepositorio, PontosRepositorio>();
        }
    }
}
