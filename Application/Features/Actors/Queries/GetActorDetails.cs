using Application.Core;
using Application.Features.Actors.DTOs;
using Application.Repositories.ActorRepository;
using Application.Utilities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Actors.Queries;

public class GetActorDetails
{
    public class Query : IRequest<Result<ActorDto>>
    {
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<ActorDto>>
    {
        private readonly IActorReadRepository _actorReadRepository;
        private readonly IMapper _mapper;

        public Handler(IActorReadRepository actorReadRepository, IMapper mapper)
        {
            _actorReadRepository = actorReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<ActorDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var actor = await _actorReadRepository
                .GetWhere(a => !a.IsDeleted)
                .ProjectTo<ActorDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (actor is null)
            {
                return Result<ActorDto>.Failure(MessageGenerator.NotFound("Actor"), 404);
            }

            return Result<ActorDto>.Success(actor);
        }
    }
}
