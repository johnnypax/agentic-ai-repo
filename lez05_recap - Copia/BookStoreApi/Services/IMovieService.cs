using BookStoreApi.Models;

namespace BookStoreApi.Services;

public interface IMovieService
{
    List<Movie> GetAll();
    Movie? GetById(int id);
    Movie Add(Movie movie);
    bool Update(int id, Movie movie);
    bool Delete(int id);
}
