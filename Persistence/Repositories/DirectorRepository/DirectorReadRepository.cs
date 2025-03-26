using Application.Repositories.DirectorRepository;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.DirectorRepository;

public class DirectorReadRepository : ReadRepository<Director>, IDirectorReadRepository
{
    public DirectorReadRepository(ApplicationDbContext context) : base(context) { }
}
