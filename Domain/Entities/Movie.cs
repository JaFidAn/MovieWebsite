using Domain.Entities.Common;

namespace Domain.Entities;

public class Movie : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int ReleaseYear { get; set; }
    public double Rating { get; set; }

    // Navigation
    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
}
