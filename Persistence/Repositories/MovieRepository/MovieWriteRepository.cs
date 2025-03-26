using Application.Repositories.MovieRepository;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.MovieRepository;

public class MovieWriteRepository : WriteRepository<Movie>, IMovieWriteRepository
{
    private readonly ApplicationDbContext _context;

    public MovieWriteRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void RemoveMovieGenres(Movie movie)
    {
        _context.MovieGenres.RemoveRange(movie.MovieGenres);
    }

    public void RemoveMovieActors(Movie movie)
    {
        _context.MovieActors.RemoveRange(movie.MovieActors);
    }
}
