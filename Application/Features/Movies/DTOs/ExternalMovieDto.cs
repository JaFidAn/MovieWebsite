namespace Application.Features.Movies.DTOs;

public class ExternalMovieDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int ReleaseYear { get; set; }
    public double Rating { get; set; }
}
