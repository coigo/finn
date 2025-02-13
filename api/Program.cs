using Infra.Database;
using Movimentacoes.Repositories;
using Movimentacoes.Repositories.Adapters;
using Movimentacoes.UseCases;
using Pessoa.Routes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<Context>();
builder.Services.AddScoped<IMovimentacaoRepository, MovimentacaoRepository>();
builder.Services.AddScoped<CriarMovimentacaoUseCase>();
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

app.UseRouting();
app.MapControllers();

Console.WriteLine("> Ligando.");
app.Run();


