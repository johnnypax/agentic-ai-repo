using BookStoreApi.Models;
using BookStoreApi.Repositories;

namespace BookStoreApi.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _repository;

    public MovieService(IMovieRepository repository)
    {
        _repository = repository;
    }

    public List<Movie> GetAll() => _repository.GetAll();
    public Movie? GetById(int id) => _repository.GetById(id);
    public Movie Add(Movie movie) => _repository.Add(movie);
    public bool Update(int id, Movie movie) => _repository.Update(id, movie);
    public bool Delete(int id) => _repository.Delete(id);
}
