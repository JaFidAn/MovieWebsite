using Domain.Entities.Common;

namespace Domain.Entities;

public class Actor : BaseEntity
{
    public string FullName { get; set; } = null!;

    // Navigation
    public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
}
