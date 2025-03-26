using Application.Repositories.MovieRepository;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.MovieRepository;

public class MovieReadRepository : ReadRepository<Movie>, IMovieReadRepository
{
    public MovieReadRepository(ApplicationDbContext context) : base(context) { }
}
