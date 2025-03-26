namespace Application.Features.Movies.DTOs;

public class MovieDto
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int ReleaseYear { get; set; }
    public double Rating { get; set; }

    public List<string> Genres { get; set; } = new();
}
