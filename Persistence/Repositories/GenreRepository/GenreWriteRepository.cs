using Application.Repositories.GenreRepository;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.GenreRepository;

public class GenreWriteRepository : WriteRepository<Genre>, IGenreWriteRepository
{
    public GenreWriteRepository(ApplicationDbContext context) : base(context) { }
}
