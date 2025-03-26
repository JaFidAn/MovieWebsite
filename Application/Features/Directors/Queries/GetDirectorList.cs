using Application.Core;
using Application.Features.Directors.DTOs;
using Application.Repositories.DirectorRepository;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Features.Directors.Queries;

public class GetDirectorList
{
    public class Query : IRequest<PagedResult<DirectorDto>>
    {
        public PaginationParams Params { get; set; } = new();
    }

    public class Handler : IRequestHandler<Query, PagedResult<DirectorDto>>
    {
        private readonly IDirectorReadRepository _directorReadRepository;
        private readonly IMapper _mapper;

        public Handler(IDirectorReadRepository directorReadRepository, IMapper mapper)
        {
            _directorReadRepository = directorReadRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<DirectorDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _directorReadRepository
                .GetWhere(d => !d.IsDeleted)
                .OrderBy(d => d.CreatedAt)
                .ProjectTo<DirectorDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            return await PagedResult<DirectorDto>.CreateAsync(
                query,
                request.Params.PageNumber,
                request.Params.PageSize,
                cancellationToken
            );
        }
    }
}
