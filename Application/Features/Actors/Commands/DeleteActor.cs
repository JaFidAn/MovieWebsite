using Application.Core;
using Application.Repositories.ActorRepository;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Actors.Commands;

public class DeleteActor
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string Id { get; set; }
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
            var actor = await _actorReadRepository
                .GetWhere(a => a.Id == request.Id && !a.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (actor is null)
            {
                return Result<Unit>.Failure(MessageGenerator.NotFound("Actor"), 404);
            }

            actor.IsDeleted = true;

            var result = await _actorWriteRepository.SaveAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure(MessageGenerator.DeletionFailed("Actor"), 400);
            }

            return Result<Unit>.Success(Unit.Value, MessageGenerator.DeletionSuccess("Actor"));
        }
    }
}
