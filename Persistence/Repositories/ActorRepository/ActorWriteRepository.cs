using Application.Repositories.ActorRepository;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.ActorRepository;

public class ActorWriteRepository : WriteRepository<Actor>, IActorWriteRepository
{
    public ActorWriteRepository(ApplicationDbContext context) : base(context) { }
}
