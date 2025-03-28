using Application.Core;
using Application.Features.Movies.DTOs;
using Application.Services;
using MediatR;

namespace Application.Features.Movies.Queries;

public class GetExternalMovies
{
    public class Query : IRequest<Result<List<ExternalMovieDto>>>
    {
        public PaginationParams Params { get; set; } = new();
    }

    public class Handler : IRequestHandler<Query, Result<List<ExternalMovieDto>>>
    {
        private readonly IExternalMovieService _externalMovieService;

        public Handler(IExternalMovieService externalMovieService)
        {
            _externalMovieService = externalMovieService;
        }

        public async Task<Result<List<ExternalMovieDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var movies = await _externalMovieService.GetPopularMoviesAsync(request.Params.PageNumber);
            return Result<List<ExternalMovieDto>>.Success(movies);
        }
    }
}
