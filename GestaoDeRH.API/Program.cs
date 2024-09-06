using GestaoDeRH.API.Config;
using GestaoDeRH.Infra.BancoDeDados;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<GestaoDeRhDbContext>(options =>
{
    options.UseSqlite($@"Data Source={Path.Combine("..", "LocalDatabase.db")}");
}, ServiceLifetime.Scoped);


//Configurar injecao de dependencia
//servicos
ServiceIoc.RegisterServices(builder.Services);
//repositorios
RepositoryIoC.RegisterRepositories(builder.Services);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
