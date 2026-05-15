using BookStoreApi.Models;
using BookStoreApi.Repositories;
using BookStoreApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddSingleton<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

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

// GET /movies - Restituisce tutti i film
app.MapGet("/movies", (IMovieService service) => service.GetAll());

// GET /movies/{id} - Restituisce un singolo film per ID
app.MapGet("/movies/{id}", (int id, IMovieService service) =>
{
    var movie = service.GetById(id);
    return movie is null
        ? Results.NotFound($"Film con ID {id} non trovato.")
        : Results.Ok(movie);
});

// POST /movies - Inserisce un nuovo film
app.MapPost("/movies", (Movie newMovie, IMovieService service) =>
{
    var created = service.Add(newMovie);
    return Results.Created($"/movies/{created.Id}", created);
});

// PUT /movies/{id} - Aggiorna un film esistente
app.MapPut("/movies/{id}", (int id, Movie updatedMovie, IMovieService service) =>
{
    return service.Update(id, updatedMovie)
        ? Results.Ok(service.GetById(id))
        : Results.NotFound($"Film con ID {id} non trovato.");
});

// DELETE /movies/{id} - Elimina un film per ID
app.MapDelete("/movies/{id}", (int id, IMovieService service) =>
{
    return service.Delete(id)
        ? Results.NoContent()
        : Results.NotFound($"Film con ID {id} non trovato.");
});

app.Run();
