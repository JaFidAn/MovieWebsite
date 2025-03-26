using Application.Core;
using Application.Features.Movies.DTOs;
using Application.Repositories.MovieRepository;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Movies.Queries;

public class GetMovieDetails
{
    public class Query : IRequest<Result<MovieDto>>
    {
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<MovieDto>>
    {
        private readonly IMovieReadRepository _movieReadRepository;
        private readonly IMapper _mapper;

        public Handler(IMovieReadRepository movieReadRepository, IMapper mapper)
        {
            _movieReadRepository = movieReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<MovieDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var movie = await _movieReadRepository
                .GetWhere(m => m.Id == request.Id && !m.IsDeleted)
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(cancellationToken);

            if (movie is null)
            {
                return Result<MovieDto>.Failure("Movie not found", 404);
            }

            var dto = _mapper.Map<MovieDto>(movie);
            return Result<MovieDto>.Success(dto);
        }
    }
}
