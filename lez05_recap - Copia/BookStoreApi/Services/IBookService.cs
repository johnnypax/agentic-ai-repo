using BookStoreApi.Models;

namespace BookStoreApi.Services;

public interface IBookService
{
    List<Book> GetAll();
    Book? GetById(int id);
    Book Add(Book book);
    bool Update(int id, Book book);
    bool Delete(int id);
}
