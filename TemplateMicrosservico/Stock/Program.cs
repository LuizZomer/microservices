using Exemplo;
using Microsoft.EntityFrameworkCore;
using Template.Infra;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Registrar o DataContext para SQLite
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar a interface IServExemplo e sua implementação ServExemplo
builder.Services.AddScoped<IServExemplo, ServExemplo>();
builder.Services.AddHttpClient();

// Registrar IServiceScopeFactory para ser injetado
builder.Services.AddSingleton<IServiceScopeFactory>(sp => sp.GetRequiredService<IServiceScopeFactory>());


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.Urls.Add("http://localhost:5001");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Certifique-se de que GeradorDeServicos.ServiceScopeFactory seja configurado corretamente
GeradorDeServicos.ServiceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

app.Run();
