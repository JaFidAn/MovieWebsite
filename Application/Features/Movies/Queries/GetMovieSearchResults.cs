using Application.Core;
using Application.Features.Movies.DTOs;
using Application.Repositories.MovieRepository;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Movies.Queries;

public class GetMovieSearchResults
{
    public class Query : IRequest<PagedResult<MovieSearchResultDto>>
    {
        public MovieSearchParams Params { get; set; } = null!;
    }

    public class Handler : IRequestHandler<Query, PagedResult<MovieSearchResultDto>>
    {
        private readonly IMovieReadRepository _movieReadRepository;

        public Handler(IMovieReadRepository movieReadRepository)
        {
            _movieReadRepository = movieReadRepository;
        }

        public async Task<PagedResult<MovieSearchResultDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _movieReadRepository
                .GetWhere(m => !m.IsDeleted)
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Params.Title))
            {
                query = query.Where(m => m.Title.ToLower().Contains(request.Params.Title.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(request.Params.Genre))
            {
                query = query.Where(m => m.MovieGenres.Any(mg =>
                    mg.Genre.Name.ToLower().Contains(request.Params.Genre.ToLower())));
            }

            if (request.Params.MinRating.HasValue)
            {
                query = query.Where(m => m.Rating >= request.Params.MinRating.Value);
            }

            var resultQuery = query
                .OrderByDescending(m => m.ReleaseYear)
                .Select(m => new MovieSearchResultDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    ReleaseYear = m.ReleaseYear,
                    Rating = m.Rating
                });

            return await PagedResult<MovieSearchResultDto>.CreateAsync(
                resultQuery,
                request.Params.PageNumber,
                request.Params.PageSize,
                cancellationToken
            );
        }
    }

}
