namespace Domain.Entities;

public class MovieGenre
{
    public string MovieId { get; set; } = null!;
    public Movie Movie { get; set; } = null!;

    public string GenreId { get; set; } = null!;
    public Genre Genre { get; set; } = null!;
}
