using Domain.Entities.Common;

namespace Domain.Entities;

public class Genre : BaseEntity
{
    public string Name { get; set; } = null!;

    // Navigation
    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
}
