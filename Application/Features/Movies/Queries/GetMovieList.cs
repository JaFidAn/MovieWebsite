using Application.Features.Movies.DTOs;
using Application.Repositories.MovieRepository;
using AutoMapper;
using MediatR;
using AutoMapper.QueryableExtensions;
using Application.Core;

namespace Application.Features.Movies.Queries;

public class GetMovieList
{
    public class Query : IRequest<PagedResult<MovieDto>>
    {
        public PaginationParams Params { get; set; } = new();
    }

    public class Handler : IRequestHandler<Query, PagedResult<MovieDto>>
    {
        private readonly IMovieReadRepository _movieReadRepository;
        private readonly IMapper _mapper;

        public Handler(IMovieReadRepository movieReadRepository, IMapper mapper)
        {
            _movieReadRepository = movieReadRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<MovieDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _movieReadRepository
                .GetWhere(m => !m.IsDeleted)
                .OrderByDescending(m => m.ReleaseYear)
                .ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            return await PagedResult<MovieDto>.CreateAsync(
                query,
                request.Params.PageNumber,
                request.Params.PageSize,
                cancellationToken
            );
        }
    }
}
