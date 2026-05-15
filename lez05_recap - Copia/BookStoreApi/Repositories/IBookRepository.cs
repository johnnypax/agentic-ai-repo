using BookStoreApi.Models;

namespace BookStoreApi.Repositories;

public interface IBookRepository
{
    List<Book> GetAll();
    Book? GetById(int id);
    Book Add(Book book);
    bool Update(int id, Book updated);
    bool Delete(int id);
}
