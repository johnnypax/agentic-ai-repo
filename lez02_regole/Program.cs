var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var books = new List<Book>
{
    new(1, "Il nome della rosa", "Umberto Eco", 1980, true),
    new(2, "Se questo e un uomo", "Primo Levi", 1947, true),
    new(3, "Il barone rampante", "Italo Calvino", 1957, false)
};

var nextBookId = books.Max(book => book.Id) + 1;

// Recupera tutti i libri.
// Esempio: GET /books
app.MapGet("/books", () => Results.Ok(books))
    .WithName("GetBooks")
    .WithSummary("Restituisce tutti i libri");

// Recupera un libro tramite identificativo.
// Esempio: GET /books/1
app.MapGet("/books/{id:int}", (int id) =>
{
    var selectedBook = books.FirstOrDefault(book => book.Id == id);

    return selectedBook is null
        ? Results.NotFound($"Libro con id {id} non trovato.")
        : Results.Ok(selectedBook);
})
    .WithName("GetBookById")
    .WithSummary("Restituisce un libro tramite id");

// Crea un nuovo libro.
// Esempio:
// POST /books
// Body: { "title": "Nuovo libro", "author": "Autore", "year": 2026, "isAvailable": true }
app.MapPost("/books", (CreateBookRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Author))
    {
        return Results.BadRequest("Titolo e autore sono obbligatori.");
    }

    var createdBook = new Book(nextBookId++, request.Title, request.Author, request.Year, request.IsAvailable);
    books.Add(createdBook);

    return Results.Created($"/books/{createdBook.Id}", createdBook);
})
    .WithName("CreateBook")
    .WithSummary("Crea un nuovo libro");

// Aggiorna un libro esistente.
// Esempio:
// PUT /books/1
// Body: { "title": "Titolo aggiornato", "author": "Autore aggiornato", "year": 2025, "isAvailable": false }
app.MapPut("/books/{id:int}", (int id, UpdateBookRequest request) =>
{
    var bookIndex = books.FindIndex(book => book.Id == id);

    if (bookIndex == -1)
    {
        return Results.NotFound($"Libro con id {id} non trovato.");
    }

    if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Author))
    {
        return Results.BadRequest("Titolo e autore sono obbligatori.");
    }

    var updatedBook = new Book(id, request.Title, request.Author, request.Year, request.IsAvailable);
    books[bookIndex] = updatedBook;

    return Results.Ok(updatedBook);
})
    .WithName("UpdateBook")
    .WithSummary("Aggiorna un libro esistente");

// Elimina un libro tramite identificativo.
// Esempio: DELETE /books/1
app.MapDelete("/books/{id:int}", (int id) =>
{
    var selectedBook = books.FirstOrDefault(book => book.Id == id);

    if (selectedBook is null)
    {
        return Results.NotFound($"Libro con id {id} non trovato.");
    }

    books.Remove(selectedBook);

    return Results.NoContent();
})
    .WithName("DeleteBook")
    .WithSummary("Elimina un libro tramite id");

app.Run();

record Book(int Id, string Title, string Author, int Year, bool IsAvailable);

record CreateBookRequest(string Title, string Author, int Year, bool IsAvailable);

record UpdateBookRequest(string Title, string Author, int Year, bool IsAvailable);
