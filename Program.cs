
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// LEMBRETE!!! Configurar o SQLite e o DbContext
// builder.Services.AddDbContext<BibliotecaContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// // MIGRAÇÕES
// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<BibliotecaContext>();
//     dbContext.Database.Migrate(); // migrações pendentes
// }

List<Livro> livros=
[
           new Livro(){Titulo="Dom Quixote", Autor="Miguel de Cervantes", AnoDePublicacao = 1600, DataDeEmprestimo = null, DataDeDevolução = null },
            new Livro(){Titulo="1984", Autor="George Orwell", AnoDePublicacao = 1949, DataDeEmprestimo = "10/09/2024", DataDeDevolução = "20/09/2024" },
            new Livro(){Titulo="O Senhor dos Anéis", Autor="J.R.R. Tolkien", AnoDePublicacao = 1954, DataDeEmprestimo = "17/05/2024", DataDeDevolução = "30/05/2024"},
            new Livro(){Titulo="Cem Anos de Solidão", Autor="Gabriel García Márquez", AnoDePublicacao = 1967, DataDeEmprestimo = null, DataDeDevolução = null },
            new Livro(){Titulo="O Grande Gatsby", Autor="F. Scott Fitzgerald", AnoDePublicacao = 1925, DataDeEmprestimo = "20/03/2024", DataDeDevolução = "29/03/2024"},
            new Livro(){Titulo="Orgulho e Preconceito", Autor="Jane Austen", AnoDePublicacao = 1813, DataDeEmprestimo = null, DataDeDevolução = null },
            new Livro(){Titulo="Moby Dick", Autor="Herman Melville", AnoDePublicacao = 1851 , DataDeEmprestimo = null, DataDeDevolução = null},
            new Livro(){Titulo="Guerra e Paz", Autor="Liev Tolstói", AnoDePublicacao = 1869 , DataDeEmprestimo = null, DataDeDevolução = null},
            new Livro(){Titulo="Crime e Castigo", Autor="Fiódor Dostoiévski", AnoDePublicacao = 1866 , DataDeEmprestimo = null, DataDeDevolução = null},
            new Livro(){Titulo="O Apanhador no Campo de Centeio", Autor="J.D. Salinger", AnoDePublicacao = 1951 , DataDeEmprestimo = null, DataDeDevolução = null}
];
List<Cliente> clientes=
[
    new Cliente() { Nome = "Maria Oliveira", Cpf = "123.456.789-00", DataDeInicio = new DateTime(2022, 5, 20) },
    new Cliente() { Nome = "João Silva", Cpf = "987.654.321-11", DataDeInicio = new DateTime(2023, 1, 10) },
    new Cliente() { Nome = "Ana Costa", Cpf = "321.654.987-22", DataDeInicio = new DateTime(2021, 9, 5) },
    new Cliente() { Nome = "Pedro Santos", Cpf = "456.789.123-33", DataDeInicio = new DateTime(2020, 7, 15) },
    new Cliente() { Nome = "Lucas Ferreira", Cpf = "654.321.987-44", DataDeInicio = new DateTime(2023, 3, 25) }

];

app.MapGet("/", ()=>"API de uma Biblioteca");


// Maria
// - Cadastro de Clientes
// - Consulta de Clientes
// - Atualização de Dados de Clientes

// Manu
// - Exclusão de Clientes
// - Cadastro de Livros

app.MapPost("/livros/cadastrar", ([FromBody] Livro novoLivro) =>
{
    if (livros.Any(l => l.Titulo == novoLivro.Titulo))
    {
        return Results.BadRequest("Um livro com esse título já existe.");
    }

    livros.Add(novoLivro);
    return Results.Created($"/livros/{novoLivro.Titulo}", novoLivro);
});


// - Consulta de Livros

app.MapGet("/livros/listar", () =>
{
    return Results.Ok(livros);
});

app.MapGet("/livros/{titulo}", ([FromRoute] string titulo) =>
{
    var livro = livros.FirstOrDefault(l => l.Titulo == titulo);

    if (livro == null)
    {
        return Results.NotFound("Livro não encontrado.");
    }

    return Results.Ok(livro);
});

// Guilherme
// - Atualização de Dados de Livros
// - Exclusão de Livros
// - Registro de Empréstimos

// Pedro
// - Registro de Devoluções

app.MapPut("/devolucao/{titulo}", ([FromRoute] string titulo) =>
{
    var livro = livros.FirstOrDefault(
        l => l.Titulo == titulo && l.DataDeEmprestimo != null && l.DataDeDevolução == null);

    if (livro == null)
    {
        return Results.NotFound("O livro não foi encontrado ou já foi devolvido!!");
    }

    livro.DataDeEmprestimo = DateTime.Now.ToString("dd/MM/yyyy");
    return Results.Ok($"A devolução do livro '{livro.Titulo}' foi registrada com sucesso!! ");
});

// - Consulta de Empréstimos Ativos
// - Reservas de Livros

app.Run();
