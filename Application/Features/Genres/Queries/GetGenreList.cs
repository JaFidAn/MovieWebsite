using Application.Core;
using Application.Features.Genres.DTOs;
using Application.Repositories.GenreRepository;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Features.Genres.Queries;

public class GetGenreList
{
    public class Query : IRequest<PagedResult<GenreDto>>
    {
        public PaginationParams Params { get; set; } = new();
    }

    public class Handler : IRequestHandler<Query, PagedResult<GenreDto>>
    {
        private readonly IGenreReadRepository _genreReadRepository;
        private readonly IMapper _mapper;

        public Handler(IGenreReadRepository genreReadRepository, IMapper mapper)
        {
            _genreReadRepository = genreReadRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<GenreDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _genreReadRepository
                .GetWhere(g => !g.IsDeleted)
                .OrderBy(g => g.CreatedAt)
                .ProjectTo<GenreDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            return await PagedResult<GenreDto>.CreateAsync(
                query,
                request.Params.PageNumber,
                request.Params.PageSize,
                cancellationToken
            );
        }
    }
}
