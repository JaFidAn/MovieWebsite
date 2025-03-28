using Application.Core;
using Application.Features.Actors.DTOs;
using Application.Repositories.ActorRepository;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Actors.Commands;

public class EditActor
{
    public class Command : IRequest<Result<Unit>>
    {
        public required EditActorDto ActorDto { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IActorReadRepository _actorReadRepository;
        private readonly IActorWriteRepository _actorWriteRepository;

        public Handler(
            IActorReadRepository actorReadRepository,
            IActorWriteRepository actorWriteRepository)
        {
            _actorReadRepository = actorReadRepository;
            _actorWriteRepository = actorWriteRepository;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var actor = await _actorReadRepository.GetByIdAsync(request.ActorDto.Id);

            if (actor is null)
            {
                return Result<Unit>.Failure(MessageGenerator.NotFound("Actor"), 404);
            }

            var duplicate = await _actorReadRepository
                .GetWhere(a => a.FullName.ToLower() == request.ActorDto.FullName.ToLower() &&
                               a.Id != request.ActorDto.Id &&
                               !a.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (duplicate is not null)
            {
                return Result<Unit>.Failure(MessageGenerator.DuplicateExists("Actor"), 400);
            }

            actor.FullName = request.ActorDto.FullName;

            var result = await _actorWriteRepository.SaveAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure(MessageGenerator.UpdateFailed("Actor"), 400);
            }

            return Result<Unit>.Success(Unit.Value, MessageGenerator.UpdateSuccess("Actor"));
        }
    }
}
