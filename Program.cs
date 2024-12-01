using BibliotecaAPI.Data;
using BibliotecaAPI.Models;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
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

app.MapGet("/api/clientes/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Clientes.Any())
    {
        return Results.Ok(ctx.Clientes.ToList());
    }
    return Results.NotFound();
});

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

app.MapGet("/api/livros/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Livros.Any())
    {
        return Results.Ok(ctx.Livros.ToList());
    }
    return Results.NotFound();
});

app.MapGet("/api/livros/buscar/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    var livro = ctx.Livros.Find(id);  // 'id' agora é do tipo 'int'
    if (livro == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(livro);
});


app.MapPost("/api/livros/cadastrar", ([FromBody] Livro livro, [FromServices] AppDataContext ctx) =>
{
    ctx.Livros.Add(livro);
    ctx.SaveChanges();
    return Results.Created($"/api/livros/{livro.LivroId}", livro);
});

app.MapPut("/api/livros/alterar/{id}", ([FromRoute] int id, [FromBody] Livro livroAlterado, [FromServices] AppDataContext ctx) =>
{
    var livro = ctx.Livros.Find(id);  // 'id' agora é do tipo 'int'
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


app.MapDelete("/api/livros/deletar/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    var livro = ctx.Livros.Find(id);  // 'id' agora é do tipo 'int'
    if (livro == null)
    {
        return Results.NotFound();
    }
    ctx.Livros.Remove(livro);
    ctx.SaveChanges();
    return Results.Ok(livro);
});


app.MapGet("/api/emprestimos/listar", ([FromServices] AppDataContext ctx) =>
{
    var emprestimosAtivos = ctx.Emprestimos
        .Include(e => e.Cliente)
        .Include(e => e.Livro)
        .Where(e => e.DataDeEmprestimo != null && e.DataDeDevolucao == null)
        .ToList();
    return Results.Ok(emprestimosAtivos);
});

app.MapPost("/api/emprestimos/cadastrar", async ([FromBody] Emprestimo novoEmprestimo, [FromServices] AppDataContext ctx) =>
{
    if (novoEmprestimo == null || novoEmprestimo.ClienteId == 0 || novoEmprestimo.LivroId == 0)
    {
        return Results.BadRequest("Dados do empréstimo são inválidos. ClienteId e LivroId são obrigatórios.");
    }

    var cliente = await ctx.Clientes.FindAsync(novoEmprestimo.ClienteId);
    var livro = await ctx.Livros.FindAsync(novoEmprestimo.LivroId);

    if (cliente == null)
    {
        return Results.NotFound($"Cliente com ID {novoEmprestimo.ClienteId} não encontrado.");
    }

    if (livro == null)
    {
        return Results.NotFound($"Livro com ID {novoEmprestimo.LivroId} não encontrado.");
    }

    // Adiciona o novo empréstimo
    ctx.Emprestimos.Add(novoEmprestimo);
    await ctx.SaveChangesAsync();

    return Results.Created($"/api/emprestimos/{novoEmprestimo.Id}", novoEmprestimo);
});


app.MapPost("/api/emprestimos/devolver", async ([FromBody] Emprestimo devolucao, [FromServices] AppDataContext ctx) =>
{
    // Verificar se o ID do empréstimo foi fornecido
    if (devolucao?.Id == null)
    {
        return Results.BadRequest("ID do empréstimo não foi fornecido.");
    }

    // Buscando o empréstimo pelo ID
    var emprestimo = await ctx.Emprestimos.FindAsync(devolucao.Id);

    // Verificar se o empréstimo foi encontrado
    if (emprestimo == null)
    {
        return Results.NotFound("Empréstimo não encontrado.");
    }

    // Verificar se o livro já foi devolvido
    if (emprestimo.DataDeDevolucao != null)
    {
        return Results.BadRequest("Este livro já foi devolvido.");
    }

    // Atualizando a data de devolução
    emprestimo.DataDeDevolucao = DateTime.Now;

    // Salvando as alterações no banco de dados
    await ctx.SaveChangesAsync();

    // Retornando a resposta com o empréstimo atualizado
    return Results.Ok(emprestimo);
});




app.UseCors("Acesso Total");

app.Run();

