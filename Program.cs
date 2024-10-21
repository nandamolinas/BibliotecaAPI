using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDataContext>();
    db.Database.EnsureCreated();

    if (!db.Clientes.Any())
    {
        var clientes = new List<Cliente>
        {
            new Cliente() { ClienteId = 1, Nome = "Maria Oliveira", Cpf = "123.456.789-00", DataDeInicio = new DateTime(2022, 5, 20) },
            new Cliente() { ClienteId = 2, Nome = "João Silva", Cpf = "987.654.321-11", DataDeInicio = new DateTime(2023, 1, 10) },
            new Cliente() { ClienteId = 3, Nome = "Ana Costa", Cpf = "321.654.987-22", DataDeInicio = new DateTime(2021, 9, 5) },
            new Cliente() { ClienteId = 4, Nome = "Pedro Santos", Cpf = "456.789.123-33", DataDeInicio = new DateTime(2020, 7, 15) },
            new Cliente() { ClienteId = 5, Nome = "Lucas Ferreira", Cpf = "654.321.987-44", DataDeInicio = new DateTime(2023, 3, 25) }
        };

        var livros = new List<Livro>
        {
            new Livro() { IdLivro = 1, Titulo = "Dom Quixote", Autor = "Miguel de Cervantes", AnoDePublicacao = 1600 },
            new Livro() { IdLivro = 2, Titulo = "1984", Autor = "George Orwell", AnoDePublicacao = 1949 },
            new Livro() { IdLivro = 3, Titulo = "O Senhor dos Anéis", Autor = "J.R.R. Tolkien", AnoDePublicacao = 1954 },
            new Livro() { IdLivro = 4, Titulo = "Cem Anos de Solidão", Autor = "Gabriel García Márquez", AnoDePublicacao = 1967 },
            new Livro() { IdLivro = 5, Titulo = "O Grande Gatsby", Autor = "F. Scott Fitzgerald", AnoDePublicacao = 1925 },
            new Livro() { IdLivro = 6, Titulo = "Orgulho e Preconceito", Autor = "Jane Austen", AnoDePublicacao = 1813 },
            new Livro() { IdLivro = 7, Titulo = "Moby Dick", Autor = "Herman Melville", AnoDePublicacao = 1851 },
            new Livro() { IdLivro = 8, Titulo = "Guerra e Paz", Autor = "Liev Tolstói", AnoDePublicacao = 1869 },
            new Livro() { IdLivro = 9, Titulo = "Crime e Castigo", Autor = "Fiódor Dostoiévski", AnoDePublicacao = 1866 },
            new Livro() { IdLivro = 10, Titulo = "O Apanhador no Campo de Centeio", Autor = "J.D. Salinger", AnoDePublicacao = 1951 }
        };

        db.Clientes.AddRange(clientes);
        db.Livros.AddRange(livros);
        db.SaveChanges();
    }
}

app.MapGet("/", () => "Bem-vindo à API da Biblioteca!");




app.MapGet("/api/clientes/listar", async (AppDataContext db) =>
{
    return Results.Ok(await db.Clientes.ToListAsync());
});

app.MapPut("/api/clientes/atualizar/{clienteId}", async ([FromRoute] int clienteId, [FromBody] Cliente clienteAlterado, AppDataContext db) =>
{
    var cliente = await db.Clientes.FindAsync(clienteId);
    if (cliente == null)
    {
        return Results.NotFound("Cliente não encontrado.");
    }

    cliente.Nome = clienteAlterado.Nome;
    cliente.Cpf = clienteAlterado.Cpf;
    cliente.DataDeInicio = clienteAlterado.DataDeInicio;

    await db.SaveChangesAsync();
    return Results.Ok(cliente);
});

app.MapDelete("/api/clientes/excluir/{clienteId}", async ([FromRoute] int clienteId, AppDataContext db) =>
{
    var cliente = await db.Clientes.FindAsync(clienteId);
    if (cliente == null)
    {
        return Results.NotFound("Cliente não encontrado.");
    }

    db.Clientes.Remove(cliente);
    await db.SaveChangesAsync();
    return Results.Ok($"Cliente {cliente.Nome} excluído com sucesso.");
});

app.MapPost("/api/clientes/cadastrar", async (Cliente cliente, AppDataContext context) =>
{
    if (await context.Clientes.AnyAsync(c => c.ClienteId == cliente.ClienteId))
    {
        return Results.BadRequest("ClienteId já existe.");
    }

    await context.Clientes.AddAsync(cliente);
    await context.SaveChangesAsync();
    return Results.Created($"/api/clientes/{cliente.ClienteId}", cliente);
});


app.MapGet("/api/livros/listar", async (AppDataContext db) =>
{
    return Results.Ok(await db.Livros.ToListAsync());
});

app.MapGet("/api/livros/{titulo}", async ([FromRoute] string titulo, AppDataContext db) =>
{
    var livro = await db.Livros.FirstOrDefaultAsync(l => l.Titulo == titulo);
    if (livro == null)
    {
        return Results.NotFound("Livro não encontrado.");
    }

    return Results.Ok(livro);
});

app.MapPut("/api/livros/atualizar/{titulo}", async ([FromRoute] string titulo, [FromBody] Livro livroAlterado, AppDataContext db) =>
{
    var livro = await db.Livros.FirstOrDefaultAsync(l => l.Titulo == titulo);
    if (livro == null)
    {
        return Results.NotFound("Livro não encontrado.");
    }

    livro.Autor = livroAlterado.Autor;
    livro.AnoDePublicacao = livroAlterado.AnoDePublicacao;
    livro.DataDeEmprestimo = livroAlterado.DataDeEmprestimo;
    livro.DataDeDevolucao = livroAlterado.DataDeDevolucao;

    await db.SaveChangesAsync();
    return Results.Ok(livro);
});

app.MapDelete("/api/livros/deletar/{titulo}", async ([FromRoute] string titulo, AppDataContext db) =>
{
    var livro = await db.Livros.FirstOrDefaultAsync(l => l.Titulo == titulo);
    if (livro == null)
    {
        return Results.NotFound("Livro não encontrado.");
    }

    db.Livros.Remove(livro);
    await db.SaveChangesAsync();
    return Results.Ok($"Livro {livro.Titulo} excluído com sucesso.");
});

app.MapPost("/api/livros/cadastrar", async (Livro livro, AppDataContext context) =>
{
    context.Livros.Add(livro);
    await context.SaveChangesAsync();
    return Results.Created($"/api/livros/{livro.IdLivro}", livro); // Use IdLivro aqui
});



app.MapPost("/api/emprestimos/cadastrar", async ([FromBody] Emprestimo novoEmprestimo, AppDataContext db) =>
{
    var cliente = await db.Clientes.FindAsync(novoEmprestimo.ClienteId);
    var livro = await db.Livros.FindAsync(novoEmprestimo.IdLivro);

    if (cliente == null)
    {
        return Results.NotFound("Cliente não encontrado.");
    }

    if (livro == null)
    {
        return Results.NotFound("Livro não encontrado.");
    }

    if (livro.DataDeEmprestimo != null)
    {
        return Results.BadRequest("Este livro já está emprestado.");
    }

    livro.DataDeEmprestimo = novoEmprestimo.DataDeEmprestimo;
    livro.DataDeDevolucao = novoEmprestimo.DataDeDevolucao;
    livro.ClienteId = cliente.ClienteId;

    await db.SaveChangesAsync();
    return Results.Ok($"Empréstimo do livro {livro.Titulo} para o cliente {cliente.Nome} registrado com sucesso.");
});

app.MapPost("/api/emprestimos/devolver", async ([FromBody] Emprestimo devolucao, AppDataContext db) =>
{
    var livro = await db.Livros.FindAsync(devolucao.IdLivro);
    if (livro == null)
    {
        return Results.NotFound("Livro não encontrado.");
    }

    livro.DataDeEmprestimo = null;
    livro.DataDeDevolucao = null;
    livro.ClienteId = null;

    await db.SaveChangesAsync();
    return Results.Ok($"Devolução do livro {livro.Titulo} registrada com sucesso.");
});

app.MapGet("/api/emprestimos/ativos", async (AppDataContext db) =>
{
    var emprestimosAtivos = await db.Livros.Where(l => l.DataDeEmprestimo != null).ToListAsync();
    return Results.Ok(emprestimosAtivos);
});

app.MapGet("/api/livros/resumo", async (AppDataContext db) =>
{
    var totalLivros = await db.Livros.CountAsync();
    var totalEmprestimos = await db.Livros.CountAsync(l => l.DataDeEmprestimo != null);
    var resumo = new
    {
        TotalLivros = totalLivros,
        TotalEmprestimos = totalEmprestimos
    };
    return Results.Ok(resumo);
});

app.Run();
