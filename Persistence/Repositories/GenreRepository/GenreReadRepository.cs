using Application.Repositories.GenreRepository;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.GenreRepository;

public class GenreReadRepository : ReadRepository<Genre>, IGenreReadRepository
{
    public GenreReadRepository(ApplicationDbContext context) : base(context) { }
}
