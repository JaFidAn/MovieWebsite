using Application.Core;
using Application.Features.Actors.DTOs;
using Application.Repositories.ActorRepository;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Actors.Commands;

public class CreateActor
{
    public class Command : IRequest<Result<string>>
    {
        public CreateActorDto ActorDto { get; set; } = null!;
    }

    public class Handler : IRequestHandler<Command, Result<string>>
    {
        private readonly IActorWriteRepository _actorWriteRepository;
        private readonly IActorReadRepository _actorReadRepository;
        private readonly IMapper _mapper;

        public Handler(
            IActorWriteRepository actorWriteRepository,
            IActorReadRepository actorReadRepository,
            IMapper mapper)
        {
            _actorWriteRepository = actorWriteRepository;
            _actorReadRepository = actorReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var existing = await _actorReadRepository
                .GetWhere(a => a.FullName.ToLower() == request.ActorDto.FullName.ToLower() && !a.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (existing is not null)
            {
                return Result<string>.Failure("Actor with the same name already exists", 400);
            }

            var actor = _mapper.Map<Actor>(request.ActorDto);

            await _actorWriteRepository.AddAsync(actor);
            var result = await _actorWriteRepository.SaveAsync() > 0;

            if (!result)
            {
                return Result<string>.Failure("Actor could not be created", 400);
            }

            return Result<string>.Success(actor.Id, "Actor added successfully");
        }
    }
}
