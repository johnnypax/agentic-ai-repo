using BookStoreApi.Models;

namespace BookStoreApi.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies = new()
    {
        new Movie { Id = 1, Title = "Il Padrino", Actors = new List<string> { "Marlon Brando", "Al Pacino" } },
        new Movie { Id = 2, Title = "Inception", Actors = new List<string> { "Leonardo DiCaprio", "Joseph Gordon-Levitt" } },
        new Movie { Id = 3, Title = "The Matrix", Actors = new List<string> { "Keanu Reeves", "Laurence Fishburne" } }
    };

    public List<Movie> GetAll() => _movies;

    public Movie? GetById(int id) => _movies.FirstOrDefault(m => m.Id == id);

    public Movie Add(Movie movie)
    {
        movie.Id = _movies.Count > 0 ? _movies.Max(m => m.Id) + 1 : 1;
        _movies.Add(movie);
        return movie;
    }

    public bool Update(int id, Movie updated)
    {
        var movie = GetById(id);
        if (movie is null) return false;
        movie.Title = updated.Title;
        movie.Actors = updated.Actors;
        return true;
    }

    public bool Delete(int id)
    {
        var movie = GetById(id);
        if (movie is null) return false;
        _movies.Remove(movie);
        return true;
    }
}
