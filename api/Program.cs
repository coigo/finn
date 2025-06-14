using Aportes.UseCases;
using Infra.Database;
using Infra.Env;
using Infra.Http.Controllers.Movimentacoes;
using Infra.Http.Middlewares;
using Infra.Repositories;
using Infra.Repositories.Adapters;
using Microsoft.AspNetCore.Diagnostics;
using Movimentacoes.Factories;
using Movimentacoes.UseCases;
using Resumos.UseCases;
using Salarios.UseCases;

LoadEnv.Load();
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls($"http://localhost:{Environment.GetEnvironmentVariable("PORT")}");

var allowLocal = "allowLocal";


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<Context>();

//Repositorios

builder.Services.AddScoped<IMovimentacaoRepository, MovimentacaoRepository>();
builder.Services.AddScoped<IResumoRepository, ResumoRepository>();
builder.Services.AddScoped<IAporteHistoricoRepository, AporteHistoricoRepository>();
builder.Services.AddScoped<IAporteRepository, AporteRepository>();
builder.Services.AddScoped<ISalarioRepository, SalarioRepository>();
builder.Services.AddSingleton<IAtivosRepository, AtivosRepository>();
builder.Services.AddHttpClient<IAtivosRepository, AtivosRepository>();

//UseCases

builder.Services.AddScoped<CriarMovimentacaoUseCase>();
builder.Services.AddScoped<BuscarMovimentacoesUseCase>();
builder.Services.AddScoped<SubtrairParcelasUseCase>();
builder.Services.AddScoped<SubtrairParcelasUseCase>();
builder.Services.AddScoped<BuscarCategoriasUseCase>();
builder.Services.AddScoped<BuscarAportesUseCase>();
builder.Services.AddScoped<BuscarMovimentacoesCategoriasUseCase>();
builder.Services.AddScoped<BuscarPendentesDoMesUseCase>();
builder.Services.AddScoped<ProcessarMovimentacoesPendentesUseCase>();

builder.Services.AddScoped<MovimentarAportesUseCase>();
builder.Services.AddScoped<BuscarSaldoUseCase>();

builder.Services.AddScoped<BuscarSalarioAtualUseCase>();
builder.Services.AddScoped<CriarSalarioUseCase>();
builder.Services.AddScoped<AdicionarSalarioAtualUseCase>();

//Factories

builder.Services.AddScoped<IMovimentacaoFactory, MovimentacaoFactory>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});



builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: allowLocal,
 policy =>
    {
        policy.WithOrigins($"{Environment.GetEnvironmentVariable("WEB_URL")}")
              .AllowAnyHeader();
    });
});


var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors(allowLocal);    

app.UseMiddleware<BasePathEnforcer>("/api");

app.UsePathBase("/api");

app.UseRouting();
app.MapControllers();

Console.WriteLine("> Ligando.");
app.Run();


