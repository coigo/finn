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

app.Use(async (context, next) => {
    Console.WriteLine("primeiro");
    await next(context);
});

app.UseRouting();
app.MapControllers();

Console.WriteLine("> Ligando.");
app.Run();


