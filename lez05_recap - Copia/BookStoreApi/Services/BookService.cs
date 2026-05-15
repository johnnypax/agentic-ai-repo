using BookStoreApi.Models;
using BookStoreApi.Repositories;

namespace BookStoreApi.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;

    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    public List<Book> GetAll() => _repository.GetAll();
    public Book? GetById(int id) => _repository.GetById(id);
    public Book Add(Book book) => _repository.Add(book);
    public bool Update(int id, Book book) => _repository.Update(id, book);
    public bool Delete(int id) => _repository.Delete(id);
}
