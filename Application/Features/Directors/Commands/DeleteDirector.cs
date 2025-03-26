using Application.Core;
using Application.Repositories.DirectorRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Directors.Commands;

public class DeleteDirector
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IDirectorReadRepository _directorReadRepository;
        private readonly IDirectorWriteRepository _directorWriteRepository;

        public Handler(
            IDirectorReadRepository directorReadRepository,
            IDirectorWriteRepository directorWriteRepository)
        {
            _directorReadRepository = directorReadRepository;
            _directorWriteRepository = directorWriteRepository;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var director = await _directorReadRepository
                .GetWhere(d => d.Id == request.Id && !d.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (director is null)
            {
                return Result<Unit>.Failure("Director not found", 404);
            }

            director.IsDeleted = true;

            var result = await _directorWriteRepository.SaveAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to delete director", 400);
            }

            return Result<Unit>.Success(Unit.Value, "Director deleted successfully");
        }
    }
}
