namespace Application.Features.Movies.DTOs;

public class MovieDetailsDto
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int ReleaseYear { get; set; }
    public double Rating { get; set; }

    public string Director { get; set; } = null!;
    public List<string> Genres { get; set; } = new();
    public List<string> Actors { get; set; } = new();
}
