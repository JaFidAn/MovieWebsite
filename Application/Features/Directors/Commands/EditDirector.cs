using Application.Core;
using Application.Features.Directors.DTOs;
using Application.Repositories.DirectorRepository;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Directors.Commands;

public class EditDirector
{
    public class Command : IRequest<Result<Unit>>
    {
        public required EditDirectorDto DirectorDto { get; set; }
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
            var director = await _directorReadRepository.GetByIdAsync(request.DirectorDto.Id);

            if (director is null)
            {
                return Result<Unit>.Failure(MessageGenerator.NotFound("Director"), 404);
            }

            var duplicate = await _directorReadRepository
                .GetWhere(d => d.FullName.ToLower() == request.DirectorDto.FullName.ToLower() &&
                               d.Id != request.DirectorDto.Id &&
                               !d.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (duplicate is not null)
            {
                return Result<Unit>.Failure(MessageGenerator.DuplicateExists("Director"), 400);
            }

            director.FullName = request.DirectorDto.FullName;

            var result = await _directorWriteRepository.SaveAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure(MessageGenerator.UpdateFailed("Director"), 400);
            }

            return Result<Unit>.Success(Unit.Value, MessageGenerator.UpdateSuccess("Director"));
        }
    }
}
