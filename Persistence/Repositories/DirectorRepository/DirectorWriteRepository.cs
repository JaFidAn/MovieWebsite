using Application.Repositories.DirectorRepository;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.DirectorRepository;

public class DirectorWriteRepository : WriteRepository<Director>, IDirectorWriteRepository
{
    public DirectorWriteRepository(ApplicationDbContext context) : base(context) { }
}
