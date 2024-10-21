using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure o DbContext
builder.Services.AddDbContext<AppDataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Inicialização de Dados
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDataContext>();
    db.Database.EnsureCreated();

    // Verifica se a tabela de Clientes está vazia
    if (!db.Clientes.Any())
    {
        List<Cliente> clientes = new List<Cliente>
        {
            new Cliente() { ClienteId = 1, Nome = "Maria Oliveira", Cpf = "123.456.789-00", DataDeInicio = new DateTime(2022, 5, 20) },
            new Cliente() { ClienteId = 2, Nome = "João Silva", Cpf = "987.654.321-11", DataDeInicio = new DateTime(2023, 1, 10) },
            new Cliente() { ClienteId = 3, Nome = "Ana Costa", Cpf = "321.654.987-22", DataDeInicio = new DateTime(2021, 9, 5) },
            new Cliente() { ClienteId = 4, Nome = "Pedro Santos", Cpf = "456.789.123-33", DataDeInicio = new DateTime(2020, 7, 15) },
            new Cliente() { ClienteId = 5, Nome = "Lucas Ferreira", Cpf = "654.321.987-44", DataDeInicio = new DateTime(2023, 3, 25) }
        };

        List<Livro> livros = new List<Livro>
        {
            new Livro() { IdLivro = 1, Titulo = "Dom Quixote", Autor = "Miguel de Cervantes", AnoDePublicacao = 1600, DataDeEmprestimo = null, DataDeDevolucao = null },
            new Livro() { IdLivro = 2, Titulo = "1984", Autor = "George Orwell", AnoDePublicacao = 1949, DataDeEmprestimo = DateTime.Now.AddDays(-10), DataDeDevolucao = DateTime.Now.AddDays(10), ClienteId = 1 },
            new Livro() { IdLivro = 3, Titulo = "O Senhor dos Anéis", Autor = "J.R.R. Tolkien", AnoDePublicacao = 1954, DataDeEmprestimo = DateTime.Now.AddDays(-7), DataDeDevolucao = DateTime.Now.AddDays(7), ClienteId = 2 },
            new Livro() { IdLivro = 4, Titulo = "Cem Anos de Solidão", Autor = "Gabriel García Márquez", AnoDePublicacao = 1967, DataDeEmprestimo = null, DataDeDevolucao = null },
            new Livro() { IdLivro = 5, Titulo = "O Grande Gatsby", Autor = "F. Scott Fitzgerald", AnoDePublicacao = 1925, DataDeEmprestimo = DateTime.Now.AddDays(-20), DataDeDevolucao = DateTime.Now.AddDays(10), ClienteId = 3 },
            new Livro() { IdLivro = 6, Titulo = "Orgulho e Preconceito", Autor = "Jane Austen", AnoDePublicacao = 1813, DataDeEmprestimo = null, DataDeDevolucao = null },
            new Livro() { IdLivro = 7, Titulo = "Moby Dick", Autor = "Herman Melville", AnoDePublicacao = 1851, DataDeEmprestimo = null, DataDeDevolucao = null },
            new Livro() { IdLivro = 8, Titulo = "Guerra e Paz", Autor = "Liev Tolstói", AnoDePublicacao = 1869, DataDeEmprestimo = null, DataDeDevolucao = null },
            new Livro() { IdLivro = 9, Titulo = "Crime e Castigo", Autor = "Fiódor Dostoiévski", AnoDePublicacao = 1866, DataDeEmprestimo = null, DataDeDevolucao = null },
            new Livro() { IdLivro = 10, Titulo = "O Apanhador no Campo de Centeio", Autor = "J.D. Salinger", AnoDePublicacao = 1951, DataDeEmprestimo = null, DataDeDevolucao = null }
        };

        db.Clientes.AddRange(clientes);
        db.Livros.AddRange(livros);
        db.SaveChanges();
    }
}

// Cadastro de Livros
app.MapPost("/api/livros/cadastrar", async ([FromBody] Livro novoLivro, AppDataContext db) =>
{
    if (await db.Livros.AnyAsync(l => l.Titulo == novoLivro.Titulo))
    {
        return Results.BadRequest("Um livro com esse título já existe.");
    }

    await db.Livros.AddAsync(novoLivro);
    await db.SaveChangesAsync();
    return Results.Created($"/api/livros/{novoLivro.Titulo}", novoLivro);
});

// Listar Livros
app.MapGet("/api/livros/listar", async (AppDataContext db) =>
{
    return Results.Ok(await db.Livros.ToListAsync());
});

// Atualização de Dados de Livros
app.MapPut("/api/livros/atualizar/{titulo}", async ([FromRoute] string titulo, [FromBody] Livro livroAlterado, AppDataContext db) =>
{
    Livro? livro = await db.Livros.FirstOrDefaultAsync(l => l.Titulo == titulo);

    if (livro == null)
    {
        return Results.NotFound("Livro não encontrado.");
    }

    livro.Titulo = livroAlterado.Titulo;
    livro.Autor = livroAlterado.Autor;
    livro.AnoDePublicacao = livroAlterado.AnoDePublicacao;
    livro.DataDeEmprestimo = livroAlterado.DataDeEmprestimo;
    livro.DataDeDevolucao = livroAlterado.DataDeDevolucao;

    await db.SaveChangesAsync();
    return Results.Ok(livro);
});



// Rotas
app.MapGet("/", () => "API de uma Biblioteca");

// Exclusão de Clientes
app.MapDelete("/api/clientes/excluir/{clienteId}", async ([FromRoute] int clienteId, AppDataContext db) =>
{
    // Busca o cliente pelo ID
    Cliente? cliente = await db.Clientes.FindAsync(clienteId);

    if (cliente == null)
    {
        return Results.NotFound("Cliente não encontrado.");
    }

    // Verifica se o cliente tem livros emprestados
    var livrosEmprestados = await db.Livros
        .Where(l => l.ClienteId == clienteId && l.DataDeEmprestimo != null && l.DataDeDevolucao == null)
        .ToListAsync();

    if (livrosEmprestados.Any())
    {
        return Results.BadRequest("O cliente possui livros emprestados e não pode ser excluído antes de devolver os livros.");
    }

    // Remove o cliente do banco de dados
    db.Clientes.Remove(cliente);
    await db.SaveChangesAsync();
    return Results.Ok($"Cliente {cliente.Nome} excluído com sucesso.");
});

// Cadastro de Clientes
app.MapPost("/api/clientes/cadastrar", async ([FromBody] Cliente novoCliente, AppDataContext db) =>
{
    if (await db.Clientes.AnyAsync(c => c.Cpf == novoCliente.Cpf))
    {
        return Results.BadRequest("Já existe um cliente com esse CPF.");
    }

    await db.Clientes.AddAsync(novoCliente);
    await db.SaveChangesAsync();
    return Results.Created($"/api/clientes/{novoCliente.ClienteId}", novoCliente);
});


// Listar Clientes
app.MapGet("/api/clientes/listar", async (AppDataContext db) =>
{
    return Results.Ok(await db.Clientes.ToListAsync());
});


// Buscar Livros
app.MapGet("/api/livros/{titulo}", async ([FromRoute] string titulo, AppDataContext db) =>
{
    var livro = await db.Livros.FirstOrDefaultAsync(l => l.Titulo == titulo);

    if (livro == null)
    {
        return Results.NotFound("Livro não encontrado.");
    }

    return Results.Ok(livro);
});

// Atualização de Dados de Clientes
app.MapPut("/api/clientes/atualizar/{clienteId}", async ([FromRoute] int clienteId, [FromBody] Cliente clienteAlterado, AppDataContext db) =>
{
    Cliente? cliente = await db.Clientes.FindAsync(clienteId);

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


// Exclusão de Livros
app.MapDelete("/api/livros/deletar/{titulo}", async ([FromRoute] string titulo, AppDataContext db) =>
{
    Livro? livro = await db.Livros.FirstOrDefaultAsync(l => l.Titulo == titulo);

    if (livro == null)
    {
        return Results.NotFound("Livro não encontrado.");
    }

    db.Livros.Remove(livro);
    await db.SaveChangesAsync();
    return Results.Ok($"Livro '{livro.Titulo}' excluído com sucesso.");
});


// Cadastro de Empréstimos
app.MapPost("/api/emprestimos/cadastrar", async ([FromBody] Emprestimo novoEmprestimo, AppDataContext db) =>
{
    var livro = await db.Livros.FirstOrDefaultAsync(l => l.Titulo == novoEmprestimo.Titulo);
    var cliente = await db.Clientes.FirstOrDefaultAsync(c => c.Nome == novoEmprestimo.ClienteNome);

    if (livro == null)
    {
        return Results.NotFound("Livro não encontrado.");
    }

    if (cliente == null)
    {
        return Results.NotFound("Cliente não encontrado.");
    }

    livro.DataDeEmprestimo = novoEmprestimo.DataDeEmprestimo;
    livro.DataDeDevolucao = novoEmprestimo.DataDeDevolucao;

    await db.Emprestimos.AddAsync(novoEmprestimo);
    await db.SaveChangesAsync();
    return Results.Created($"/emprestimos/{novoEmprestimo.IdEmprestimo}", novoEmprestimo);
});

// Consulta de Empréstimos Ativos
app.MapGet("/api/emprestimos/ativos", async (AppDataContext db) =>
{
    var emprestimosAtivos = await db.Emprestimos
        .Where(e => e.DataDeDevolucao == null) // Filtrar apenas os empréstimos sem data de devolução
        .ToListAsync();

    if (emprestimosAtivos.Count == 0)
    {
        return Results.NotFound("Nenhum empréstimo ativo encontrado.");
    }

    return Results.Ok(emprestimosAtivos);
});

// Registro de Devoluções
app.MapPost("/api/emprestimos/devolver", async ([FromBody] Emprestimo devolucao, AppDataContext db) =>
{
    // Verifica se o empréstimo existe e está ativo
    var emprestimoAtivo = await db.Emprestimos
        .FirstOrDefaultAsync(e => e.IdEmprestimo == devolucao.IdEmprestimo && e.DataDeDevolucao == null);

    if (emprestimoAtivo == null)
    {
        return Results.NotFound("Empréstimo não encontrado ou já devolvido.");
    }

    // Atualiza a data de devolução
    emprestimoAtivo.DataDeDevolucao = DateTime.Now;

    // Atualiza as informações do livro
    var livro = await db.Livros.FirstOrDefaultAsync(l => l.Titulo == emprestimoAtivo.Titulo);
    if (livro != null)
    {
        livro.DataDeEmprestimo = null; // Reseta a data de empréstimo
        livro.DataDeDevolucao = null;  // Reseta a data de devolução
    }

    await db.SaveChangesAsync();
    return Results.Ok($"Livro '{livro.Titulo}' devolvido com sucesso.");
});


// Inicia a aplicação
app.Run();
