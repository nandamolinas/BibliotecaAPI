using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

var builder = WebApplication.CreateBuilder(args);

// LEMBRETE!!! Configurar o SQLite e o DbContext
builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// MIGRAÇÕES
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BibliotecaContext>();
    dbContext.Database.Migrate(); // migrações pendentes
}


// Maria
// - Cadastro de Clientes
// - Consulta de Clientes
// - Atualização de Dados de Clientes

// Manu
// - Exclusão de Clientes
// - Cadastro de Livros
// - Consulta de Livros

// !!!! Pedro: manu já criei a tabela de livros na pasta models pra fazer a minha parte cria em cima disso!

// Guilherme
// - Atualização de Dados de Livros
// - Exclusão de Livros
// - Registro de Empréstimos

// Pedro
// - Registro de Devoluções
// - Consulta de Empréstimos Ativos
// - Reservas de Livros

app.Run();
