using BookStoreApi.Models;

namespace BookStoreApi.Repositories;

public class BookRepository : IBookRepository
{
    private readonly List<Book> _books = new()
    {
        new Book { Id = 1, Title = "Il nome della rosa", Author = "Umberto Eco", Year = 1980 },
        new Book { Id = 2, Title = "1984", Author = "George Orwell", Year = 1949 },
        new Book { Id = 3, Title = "Il Signore degli Anelli", Author = "J.R.R. Tolkien", Year = 1954 }
    };

    public List<Book> GetAll() => _books;

    public Book? GetById(int id) => _books.FirstOrDefault(b => b.Id == id);

    public Book Add(Book book)
    {
        book.Id = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1;
        _books.Add(book);
        return book;
    }

    public bool Update(int id, Book updated)
    {
        var book = GetById(id);
        if (book is null) return false;
        book.Title = updated.Title;
        book.Author = updated.Author;
        book.Year = updated.Year;
        return true;
    }

    public bool Delete(int id)
    {
        var book = GetById(id);
        if (book is null) return false;
        _books.Remove(book);
        return true;
    }
}
