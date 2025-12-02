using System.Globalization;
using Aportes.UseCases;
using Infra.Database;
using Infra.Env;
using Infra.Http.Middlewares;
using Infra.Repositories;
using Infra.Repositories.Adapters;
using Microsoft.EntityFrameworkCore;
using Movimentacoes.Factories;
using Movimentacoes.UseCases;
using Saldos.UseCases;
using Salarios.UseCases;
using System.Globalization;

LoadEnv.Load(); 

Console.WriteLine("Args recebidos:");
foreach (var arg in args)
{
    Console.WriteLine($"- {arg}");
}

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls($"http://0.0.0.0:{Environment.GetEnvironmentVariable("PORT")}");

var allowLocal = "allowLocal";

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

Console.WriteLine(DateTime.Now);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddScoped<Context>();

//Repositorios

builder.Services.AddScoped<IMovimentacaoRepository, MovimentacaoRepository>();
builder.Services.AddScoped<ISaldoRepository, SaldoRepository>();
builder.Services.AddScoped<IAporteHistoricoRepository, AporteHistoricoRepository>();
builder.Services.AddScoped<IAporteRepository, AporteRepository>();
builder.Services.AddScoped<ISalarioRepository, SalarioRepository>();
builder.Services.AddSingleton<IAtivosRepository, AtivosRepository>();
builder.Services.AddHttpClient<IAtivosRepository, AtivosRepository>();

//UseCases

builder.Services.AddScoped<CriarMovimentacaoUseCase>();
builder.Services.AddScoped<BuscarMovimentacoesUseCase>();
builder.Services.AddScoped<DesfazerMovimentacaoUseCase>();
builder.Services.AddScoped<SubtrairParcelasUseCase>();
builder.Services.AddScoped<SubtrairParcelasUseCase>();
builder.Services.AddScoped<BuscarCategoriasUseCase>();
builder.Services.AddScoped<BuscarAportesUseCase>();
builder.Services.AddScoped<BuscarAportesHistoricoUseCase>();
builder.Services.AddScoped<BuscarMovimentacoesCategoriasUseCase>();
builder.Services.AddScoped<BuscarPendentesDoMesUseCase>();
builder.Services.AddScoped<ProcessarMovimentacoesPendentesUseCase>();
builder.Services.AddScoped<BuscarPersistenteUseCase>();

builder.Services.AddScoped<DeletarPersistenteUseCase>();
builder.Services.AddScoped<EditarPersistenteUseCase>();

builder.Services.AddScoped<MovimentarAportesUseCase>();
builder.Services.AddScoped<MovimentarAportesPorIdentificadorUseCase>();
builder.Services.AddScoped<BuscarSaldoUseCase>();
builder.Services.AddScoped<EditarSaldoUseCase>();

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
              .AllowAnyMethod()
              .AllowCredentials()
              .AllowAnyHeader();
    });
});


var app = builder.Build();

if (args.Contains("--migrate"))
{
    Console.WriteLine("Aplicando migracoes");
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<Context>();
    db.Database.Migrate();
    Console.WriteLine("Finalizado.");

    return;
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors(allowLocal);

app.UseRouting();
app.MapControllers();

app.MapFallbackToFile("index.html");

Console.WriteLine("> Ligando.");
app.Run();


