var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Lista in memoria che funge da "database"
var books = new List<Book>
{
    new Book { Id = 1, Title = "Il nome della rosa", Author = "Umberto Eco", Year = 1980 },
    new Book { Id = 2, Title = "1984", Author = "George Orwell", Year = 1949 },
    new Book { Id = 3, Title = "Il Signore degli Anelli", Author = "J.R.R. Tolkien", Year = 1954 }
};

// GET /books - Restituisce tutti i libri
app.MapGet("/books", () => books);

// GET /books/{id} - Restituisce un singolo libro per ID
app.MapGet("/books/{id}", (int id) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);

    if (book is null)
        return Results.NotFound($"Libro con ID {id} non trovato.");

    return Results.Ok(book);
});

// POST /books - Inserisce un nuovo libro
app.MapPost("/books", (Book newBook) =>
{
    // Genera automaticamente un nuovo ID
    newBook.Id = books.Count > 0 ? books.Max(b => b.Id) + 1 : 1;
    books.Add(newBook);

    return Results.Created($"/books/{newBook.Id}", newBook);
});

app.Run();

// Modello Book
class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
}
