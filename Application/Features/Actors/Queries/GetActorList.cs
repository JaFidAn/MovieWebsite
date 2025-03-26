using Application.Core;
using Application.Features.Actors.DTOs;
using Application.Repositories.ActorRepository;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Features.Actors.Queries;

public class GetActorList
{
    public class Query : IRequest<PagedResult<ActorDto>>
    {
        public PaginationParams Params { get; set; } = new();
    }

    public class Handler : IRequestHandler<Query, PagedResult<ActorDto>>
    {
        private readonly IActorReadRepository _actorReadRepository;
        private readonly IMapper _mapper;

        public Handler(IActorReadRepository actorReadRepository, IMapper mapper)
        {
            _actorReadRepository = actorReadRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ActorDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _actorReadRepository
                .GetWhere(a => !a.IsDeleted)
                .OrderBy(a => a.CreatedAt)
                .ProjectTo<ActorDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            return await PagedResult<ActorDto>.CreateAsync(
                query,
                request.Params.PageNumber,
                request.Params.PageSize,
                cancellationToken
            );
        }
    }
}
