using Application.Features.Movies.DTOs;

namespace Application.Services;

public interface IExternalMovieService
{
    Task<List<ExternalMovieDto>> GetPopularMoviesAsync(int page = 1);
}
