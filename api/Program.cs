using Aportes.UseCases;
using Infra.Database;
using Infra.Repositories;
using Infra.Repositories.Adapters;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Movimentacoes.UseCases;

var builder = WebApplication.CreateBuilder(args);
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

//UseCases

builder.Services.AddScoped<CriarMovimentacaoUseCase>();
builder.Services.AddScoped<BuscarMovimentacoesUseCase>();
builder.Services.AddScoped<SubtrairParcelasUseCase>();
builder.Services.AddScoped<SubtrairParcelasUseCase>();
builder.Services.AddScoped<BuscarCategoriasUseCase>();

builder.Services.AddScoped<MovimentarAportesUseCase>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});



builder.Services.AddCors( opt => {
    opt.AddPolicy(name: allowLocal, 
 policy =>
    {
        policy.WithOrigins("http://localhost:3000") 
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())    
{
    app.UseSwagger();
    app.UseSwaggerUI();
}   

app.UseExceptionHandler( appError => {

    appError.Run( async context => {
        var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
        
        if ( exceptionHandler != null ) {
            Console.WriteLine(exceptionHandler.Path);

            var exceptionType = exceptionHandler.Error;

            if ( exceptionType is ArgumentException ) {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Argumento inválido.");
            }
            else {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Erro Interno.");
            }

        }
    });

});
app.UseCors(allowLocal);

app.UseRouting();

app.MapControllers();

Console.WriteLine("> Ligando.");
app.Run();


