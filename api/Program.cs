using Aportes.UseCases;
using Infra.Database;
using Infra.Env;
using Infra.Http.Controllers.Movimentacoes;
using Infra.Http.Middlewares;
using Infra.Repositories;
using Infra.Repositories.Adapters;
using Microsoft.AspNetCore.Diagnostics;
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

builder.Services.AddScoped<MovimentarAportesUseCase>();
builder.Services.AddScoped<BuscarSaldoUseCase>();

builder.Services.AddScoped<BuscarSalarioAtualUseCase>();
builder.Services.AddScoped<CriarSalarioUseCase>();

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

// Configure the HTTP equest pipeline.


app.UseCors(allowLocal);    

app.UseMiddleware<BasePathEnforcer>("/api");
app.UsePathBase("/api");
app.UseRouting();

app.UseExceptionHandler(appError =>
{

    appError.Run(async context =>
    {
        var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandler != null)
        {

            var exceptionType = exceptionHandler.Error;
            Console.WriteLine(exceptionHandler.Endpoint);
            Console.WriteLine(exceptionHandler.Error);
            Console.WriteLine(exceptionHandler.Path);
            if (exceptionType is KeyNotFoundException)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("O que você estava buscando não encontrado.");
            }
            if (exceptionType is ArgumentException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Argumento inválido.");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Erro Interno.");
            }

        }
    });

});

app.MapControllers();

Console.WriteLine("> Ligando.");
app.Run();


