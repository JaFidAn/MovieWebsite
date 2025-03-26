using Application.Repositories.ActorRepository;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.ActorRepository;

public class ActorReadRepository : ReadRepository<Actor>, IActorReadRepository
{
    public ActorReadRepository(ApplicationDbContext context) : base(context) { }
}
