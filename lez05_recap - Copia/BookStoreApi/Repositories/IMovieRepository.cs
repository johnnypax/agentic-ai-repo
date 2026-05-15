using BookStoreApi.Models;

namespace BookStoreApi.Repositories;

public interface IMovieRepository
{
    List<Movie> GetAll();
    Movie? GetById(int id);
    Movie Add(Movie movie);
    bool Update(int id, Movie updated);
    bool Delete(int id);
}
