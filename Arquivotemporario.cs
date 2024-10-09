//eu coloquei num arquivo isolado,pq ainda nao sei onde organizar e colocar certinho

var app = builder.Build();

// Middleware para uso de rotas
app.UseRouting();

app.UseAuthorization();

app.MapControllers(); // Adicione esta linha para mapear os controladores

// Migrações
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BibliotecaContext>();
    dbContext.Database.Migrate(); // migrações pendentes
}

app.Run();
