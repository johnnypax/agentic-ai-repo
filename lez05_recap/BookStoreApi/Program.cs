using BookStoreApi.Models;
using BookStoreApi.Repositories;
using BookStoreApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

// GET /books - Restituisce tutti i libri
app.MapGet("/books", (IBookService service) => service.GetAll());

// GET /books/{id} - Restituisce un singolo libro per ID
app.MapGet("/books/{id}", (int id, IBookService service) =>
{
    var book = service.GetById(id);
    return book is null
        ? Results.NotFound($"Libro con ID {id} non trovato.")
        : Results.Ok(book);
});

// POST /books - Inserisce un nuovo libro
app.MapPost("/books", (Book newBook, IBookService service) =>
{
    var created = service.Add(newBook);
    return Results.Created($"/books/{created.Id}", created);
});

// PUT /books/{id} - Aggiorna un libro esistente
app.MapPut("/books/{id}", (int id, Book updatedBook, IBookService service) =>
{
    return service.Update(id, updatedBook)
        ? Results.Ok(service.GetById(id))
        : Results.NotFound($"Libro con ID {id} non trovato.");
});

// DELETE /books/{id} - Elimina un libro per ID
app.MapDelete("/books/{id}", (int id, IBookService service) =>
{
    return service.Delete(id)
        ? Results.NoContent()
        : Results.NotFound($"Libro con ID {id} non trovato.");
});

app.Run();
