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

var users = new List<User>
{
    new User { Id = 1, Name = "Alice Rossi", Email = "alice@example.com" },
    new User { Id = 2, Name = "Bob Bianchi", Email = "bob@example.com" }
};

// GET /users - Restituisce tutti gli utenti
app.MapGet("/users", () => users);

// GET /users/{id} - Restituisce un singolo utente per ID
app.MapGet("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);

    if (user is null)
        return Results.NotFound($"Utente con ID {id} non trovato.");

    return Results.Ok(user);
});

// POST /users - Inserisce un nuovo utente
app.MapPost("/users", (User newUser) =>
{
    newUser.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
    users.Add(newUser);

    return Results.Created($"/users/{newUser.Id}", newUser);
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

// Modello User
class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
