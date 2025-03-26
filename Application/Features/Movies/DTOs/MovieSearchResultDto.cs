namespace Application.Features.Movies.DTOs;

public class MovieSearchResultDto
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int ReleaseYear { get; set; }
    public double Rating { get; set; }
}
