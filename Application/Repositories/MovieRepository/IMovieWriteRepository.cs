using Domain.Entities;

namespace Application.Repositories.MovieRepository;

public interface IMovieWriteRepository : IWriteRepository<Movie>
{
    void RemoveMovieGenres(Movie movie);
    void RemoveMovieActors(Movie movie);
}
