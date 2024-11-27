using Microsoft.EntityFrameworkCore;
using MyProject;
using Template.Infra;

var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os ao cont�iner DI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Registra a interface e implementa��o do servi�o
builder.Services.AddScoped<IServProducts, ServProducts>();

// Configura o GeradorDeServicos
GeradorDeServicos.Configure(builder.Services);

var app = builder.Build();

// Configura��o do pipeline de requisi��o
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
