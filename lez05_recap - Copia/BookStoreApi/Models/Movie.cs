namespace BookStoreApi.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<string> Actors { get; set; } = new();
}
