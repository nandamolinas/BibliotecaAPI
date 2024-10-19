
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
List<Cliente> clientes=
[
    new Cliente() { ClienteId = 1, Nome = "Maria Oliveira", Cpf = "123.456.789-00", DataDeInicio = new DateTime(2022, 5, 20) },
    new Cliente() { ClienteId = 2,Nome = "João Silva", Cpf = "987.654.321-11", DataDeInicio = new DateTime(2023, 1, 10) },
    new Cliente() { ClienteId = 3,Nome = "Ana Costa", Cpf = "321.654.987-22", DataDeInicio = new DateTime(2021, 9, 5) },
    new Cliente() { ClienteId = 4,Nome = "Pedro Santos", Cpf = "456.789.123-33", DataDeInicio = new DateTime(2020, 7, 15) },
    new Cliente() { ClienteId = 5,Nome = "Lucas Ferreira", Cpf = "654.321.987-44", DataDeInicio = new DateTime(2023, 3, 25) }

];
List<Livro> livros=
[
           new Livro(){Titulo="Dom Quixote", Autor="Miguel de Cervantes", AnoDePublicacao = 1600, DataDeEmprestimo = null, DataDeDevolucao = null, Cliente=null },
            new Livro(){Titulo="1984", Autor="George Orwell", AnoDePublicacao = 1949, DataDeEmprestimo = "10/09/2024", DataDeDevolucao = "20/09/2024", Cliente = buscarPeloId(1) },
            new Livro(){Titulo="O Senhor dos Anéis", Autor="J.R.R. Tolkien", AnoDePublicacao = 1954, DataDeEmprestimo = "17/05/2024",DataDeDevolucao = "30/05/2024", Cliente = buscarPeloId(2)},
            new Livro(){Titulo="Cem Anos de Solidão", Autor="Gabriel García Márquez", AnoDePublicacao = 1967, DataDeEmprestimo = null, DataDeDevolucao = null },
            new Livro(){Titulo="O Grande Gatsby", Autor="F. Scott Fitzgerald", AnoDePublicacao = 1925, DataDeEmprestimo = "20/03/2024", DataDeDevolucao = "29/03/2024",Cliente = buscarPeloId(3)},
            new Livro(){Titulo="Orgulho e Preconceito", Autor="Jane Austen", AnoDePublicacao = 1813, DataDeEmprestimo = null, DataDeDevolucao = null, Cliente=null },
            new Livro(){Titulo="Moby Dick", Autor="Herman Melville", AnoDePublicacao = 1851 , DataDeEmprestimo = null, DataDeDevolucao = null, Cliente=null},
            new Livro(){Titulo="Guerra e Paz", Autor="Liev Tolstói", AnoDePublicacao = 1869 , DataDeEmprestimo = null, DataDeDevolucao = null, Cliente=null},
            new Livro(){Titulo="Crime e Castigo", Autor="Fiódor Dostoiévski", AnoDePublicacao = 1866 , DataDeEmprestimo = null, DataDeDevolucao = null, Cliente=null},
            new Livro(){Titulo="O Apanhador no Campo de Centeio", Autor="J.D. Salinger", AnoDePublicacao = 1951 , DataDeEmprestimo = null, DataDeDevolucao = null, Cliente=null}
];

app.MapGet("/", ()=>"API de uma Biblioteca");

Cliente? buscarPeloId(int clienteId){
    return clientes.Find(x => x.ClienteId == clienteId);
}


// Maria
// - Cadastro de Clientes
// - Consulta de Clientes
// - Atualização de Dados de Clientes

// Manu parte 1 : Exclusao dos Clientes
// Rota para deletar um cliente

app.MapDelete("/api/clientes/excluir/{clienteId}", ([FromRoute] int clienteId) =>
{
    // Busca o cliente pelo ID
    Cliente? cliente = buscarPeloId(clienteId);
    
    if (cliente == null)
    {
        return Results.NotFound("Cliente não encontrado.");
    }

    // Verifica se o cliente tem livros emprestados
    var livrosEmprestados = livros.Where(l => l.Cliente?.ClienteId == clienteId && l.DataDeEmprestimo != null && l.DataDeDevolucao == null).ToList();

    if (livrosEmprestados.Any())
    {
        return Results.BadRequest("O cliente possui livros emprestados e não pode ser excluído antes de devolver os livros.");
    }

    // Remove o cliente da lista de clientes
    clientes.Remove(cliente);
    return Results.Ok($"Cliente {cliente.Nome} excluído com sucesso.");
});
//Manu parte 2: Cadastro de Livros
app.MapPost("/api/livros/cadastrar", ([FromBody] Livro novoLivro) =>
{
    // Verifica se já existe um livro com o mesmo título
    if (livros.Any(l => l.Titulo == novoLivro.Titulo))
    {
        return Results.BadRequest("Um livro com esse título já existe.");
    }

    // Adiciona o novo livro à lista
    livros.Add(novoLivro);
    return Results.Created($"/livros/{novoLivro.Titulo}", novoLivro);
});
//fim da minha parte
app.MapGet("/api/livros/listar", () =>
{
    return Results.Ok(livros);
});



app.MapPost("/api/livros/cadastrar", ([FromBody] Livro novoLivro) =>
{
    if (livros.Any(l => l.Titulo == novoLivro.Titulo))
    {
        return Results.BadRequest("Um livro com esse título já existe.");
    }

    livros.Add(novoLivro);
    return Results.Created($"/livros/{novoLivro.Titulo}", novoLivro);
});


// - Consulta de Livros

app.MapGet("/api/livros/listar", () =>
{
    return Results.Ok(livros);
});
//Buscar Livros

app.MapGet("/api/livros/{titulo}", ([FromRoute] string titulo) =>
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
app.MapPut("/api/atualizar/{NomeLivro}",([FromRoute] string NomeLivro, [FromBody] Livro livroAlterado)=>
{
    Livro? livro= livros.Find(x => x.Titulo == NomeLivro);
    if(livro == null){
        return Results.NotFound();
    }
    livro.Titulo = livroAlterado.Titulo;
    livro.Autor = livroAlterado.Autor;
    livro.AnoDePublicacao = livroAlterado.AnoDePublicacao;
    livro.DataDeEmprestimo = livroAlterado.DataDeEmprestimo;
    livro.DataDeDevolucao = livroAlterado.DataDeDevolucao;
    livro.Cliente = livro.Cliente;
    return Results.Ok(livro);
});


// - Exclusão de Livros
app.MapDelete("/api/deletar/{nomeLivro}",([FromRoute] string nomeLivro)=>
{
    Livro? livro = livros.Find(x => x.Titulo == nomeLivro);
     if(livro == null){
        return Results.NotFound();
    }
    livros.Remove(livro);
    return Results.Ok(livro);
});

// - Registro de Empréstimos
app.MapPut("/api/registraEmprestimo/{nomeLivro}/{EmprestimoCliente}", ([FromRoute] string nomeLivro, [FromRoute] string EmprestimoCliente, [FromBody] Livro livroAlterado) =>
{
    Livro? livro = livros.Find(x => x.Titulo == nomeLivro);
    Cliente? cliente = clientes.Find(c => c.Nome == EmprestimoCliente);
    
    if (cliente == null)
    {
        return Results.NotFound("Cliente não cadastrado");
    }
    if (livro == null)
    {
        return Results.NotFound();
    }
    if (livro.DataDeEmprestimo != null)
    {
        return Results.BadRequest("O livro já foi emprestado");
    }

    livro.DataDeEmprestimo = DateTime.Now.ToString(); // Atribuição direta, se DataDeEmprestimo for DateTime
    livro.DataDeDevolucao = livroAlterado.DataDeDevolucao; // Verifique o nome
    livro.Cliente = cliente; // Usar cliente diretamente

    return Results.Ok(livro);
});



// Pedro
// - Registro de Devoluções

app.MapPut("/api/devolucao/{titulo}", ([FromRoute] string titulo) =>
{
    var livro = livros.FirstOrDefault(
        l => l.Titulo == titulo && l.DataDeEmprestimo != null && l.DataDeDevolucao == null);

    if (livro == null)
    {
        return Results.NotFound("O livro não foi encontrado ou já foi devolvido!!");
    }

    livro.DataDeDevolucao = DateTime.Now.ToString("dd/MM/yyyy");
    return Results.Ok($"A devolução do livro '{livro.Titulo}' foi registrada com sucesso!! ");
});

// - Consulta de Empréstimos Ativos
// - Reservas de Livros

app.Run();
