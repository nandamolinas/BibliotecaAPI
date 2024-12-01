using API.Models;
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext
builder.Services.AddDbContext<AppDataContext>();

builder.Services.AddCors(
    options =>
        options.AddPolicy("Acesso Total",
            configs => configs
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod())
);

var app = builder.Build();
     
app.MapGet("/", () => "API da Biblioteca");

// GET: /api/clientes/listar
app.MapGet("/api/clientes/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Clientes.Any())
    {
        return Results.Ok(ctx.Clientes.ToList());
    }
    return Results.NotFound();
});

// POST: /api/clientes/cadastrar
app.MapPost("/api/clientes/cadastrar", ([FromBody] Cliente cliente, [FromServices] AppDataContext ctx) =>
{
    if (ctx.Clientes.Any(c => c.ClienteId == cliente.ClienteId))
    {
        return Results.BadRequest("Cliente já cadastrado.");
    }
    ctx.Clientes.Add(cliente);
    ctx.SaveChanges();
    return Results.Created($"/api/clientes/{cliente.ClienteId}", cliente);
});

// PUT: /api/clientes/atualizar/{id}
app.MapPut("/api/clientes/atualizar/{id}", ([FromRoute] int id, [FromBody] Cliente clienteAlterado, [FromServices] AppDataContext ctx) =>
{
    var cliente = ctx.Clientes.Find(id);
    if (cliente == null)
    {
        return Results.NotFound();
    }
    cliente.Nome = clienteAlterado.Nome;
    cliente.Cpf = clienteAlterado.Cpf;
    cliente.DataDeInicio = clienteAlterado.DataDeInicio;

    ctx.SaveChanges();
    return Results.Ok(cliente);
});

// DELETE: /api/clientes/excluir/{id}
app.MapDelete("/api/clientes/excluir/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    var cliente = ctx.Clientes.Find(id);
    if (cliente == null)
    {
        return Results.NotFound();
    }
    ctx.Clientes.Remove(cliente);
    ctx.SaveChanges();
    return Results.Ok(cliente);
});

// GET: /api/livros/listar
app.MapGet("/api/livros/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Livros.Any())
    {
        return Results.Ok(ctx.Livros.ToList());
    }
    return Results.NotFound();
});

// GET: /api/livros/buscar/{id}
app.MapGet("/api/livros/buscar/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    var livro = ctx.Livros.Find(id);
    if (livro == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(livro);
});

// POST: /api/livros/cadastrar
app.MapPost("/api/livros/cadastrar", ([FromBody] Livro livro, [FromServices] AppDataContext ctx) =>
{
    ctx.Livros.Add(livro);
    ctx.SaveChanges();
    return Results.Created($"/api/livros/{livro.LivroId}", livro);
});

// PUT: /api/livros/alterar/{id}
app.MapPut("/api/livros/alterar/{id}", ([FromRoute] int id, [FromBody] Livro livroAlterado, [FromServices] AppDataContext ctx) =>
{
    var livro = ctx.Livros.Find(id);
    if (livro == null)
    {
        return Results.NotFound();
    }
    livro.Titulo = livroAlterado.Titulo;
    livro.Autor = livroAlterado.Autor;
    livro.AnoDePublicacao = livroAlterado.AnoDePublicacao;
    ctx.SaveChanges();
    return Results.Ok(livro);
});

// DELETE: /api/livros/excluir/{id}
app.MapDelete("/api/livros/deletar/{id}", ([FromRoute] string id, [FromServices] AppDataContext ctx) =>
{
    Livro? livro = ctx.Livros.Find(id);
    if (livro == null)
    {
        return Results.NotFound();
    }
    ctx.Livros.Remove(livro);
    ctx.SaveChanges();
    return Results.Ok(livro);
});
//Listar empretimos
app.MapGet("/api/emprestimos/listar", ([FromServices] AppDataContext ctx) =>
{
    var emprestimosAtivos = ctx.Emprestimos.Where(e => e.DataDeEmprestimo != null && e.DataDeDevolucao == null).ToList();
    return Results.Ok(emprestimosAtivos);
});


// POST: /api/emprestimos/cadastrar
app.MapPost("/api/emprestimos/cadastrar", ([FromBody] Emprestimo novoEmprestimo, [FromServices] AppDataContext ctx) =>
{
    var cliente = ctx.Clientes.Find(novoEmprestimo.ClienteId);
    var livro = ctx.Livros.Find(novoEmprestimo.LivroId);

    if (cliente == null || livro == null)
    {
        return Results.NotFound("Cliente ou livro não encontrado.");
    }
    if (livro.DataDeEmprestimo != null)
    {
        return Results.BadRequest("Livro já está emprestado.");
    }

    livro.DataDeEmprestimo = novoEmprestimo.DataDeEmprestimo;
    livro.ClienteId = cliente.ClienteId;
    ctx.SaveChanges();

    return Results.Ok(livro);
});


// POST: /api/emprestimos/devolver
app.MapPost("/api/emprestimos/devolver", ([FromBody] Emprestimo devolucao, [FromServices] AppDataContext ctx) =>
{
    var emprestimo = ctx.Emprestimos.Find(devolucao.LivroId);  // Buscar na tabela Emprestimos
    if (emprestimo == null)
    {
        return Results.NotFound();
    }

    emprestimo.DataDeDevolucao = DateTime.Now;  // Registrar a data de devolução
    ctx.SaveChanges();

    return Results.Ok(emprestimo);
});


app.UseCors("Acesso Total");
app.Run();

