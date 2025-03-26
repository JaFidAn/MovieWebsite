namespace Application.Features.Movies.DTOs;

public class CreateMovieDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int ReleaseYear { get; set; }
    public double Rating { get; set; }

    public List<string> GenreIds { get; set; } = new();
}
