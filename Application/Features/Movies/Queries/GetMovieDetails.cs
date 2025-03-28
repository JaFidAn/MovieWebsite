using Application.Core;
using Application.Features.Movies.DTOs;
using Application.Repositories.MovieRepository;
using Application.Utilities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Movies.Queries;

public class GetMovieDetails
{
    public class Query : IRequest<Result<MovieDetailsDto>>
    {
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<MovieDetailsDto>>
    {
        private readonly IMovieReadRepository _movieReadRepository;
        private readonly IMapper _mapper;

        public Handler(IMovieReadRepository movieReadRepository, IMapper mapper)
        {
            _movieReadRepository = movieReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<MovieDetailsDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var movie = await _movieReadRepository
                .GetWhere(m => m.Id == request.Id && !m.IsDeleted)
                .Include(m => m.Director)
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(cancellationToken);

            if (movie is null)
            {
                return Result<MovieDetailsDto>.Failure(MessageGenerator.NotFound("Movie"), 404);
            }

            var dto = _mapper.Map<MovieDetailsDto>(movie);

            return Result<MovieDetailsDto>.Success(dto);
        }
    }
}
