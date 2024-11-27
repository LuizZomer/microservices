using Microsoft.EntityFrameworkCore;
using MyProject;
using Template.Infra;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner DI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Registra a interface e implementação do serviço
builder.Services.AddScoped<IServProducts, ServProducts>();

// Configura o GeradorDeServicos
GeradorDeServicos.Configure(builder.Services);

var app = builder.Build();

// Configuração do pipeline de requisição
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
