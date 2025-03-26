using Domain.Entities.Common;

namespace Domain.Entities;

public class Director : BaseEntity
{
    public string FullName { get; set; } = null!;

    // Navigation
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
