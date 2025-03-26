using Application.Core;
using Application.Repositories.GenreRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Genres.Commands;

public class DeleteGenre
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IGenreReadRepository _genreReadRepository;
        private readonly IGenreWriteRepository _genreWriteRepository;

        public Handler(
            IGenreReadRepository genreReadRepository,
            IGenreWriteRepository genreWriteRepository)
        {
            _genreReadRepository = genreReadRepository;
            _genreWriteRepository = genreWriteRepository;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var genre = await _genreReadRepository
                .GetWhere(g => g.Id == request.Id && !g.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (genre is null)
            {
                return Result<Unit>.Failure("Genre not found", 404);
            }

            genre.IsDeleted = true;

            var result = await _genreWriteRepository.SaveAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to delete genre", 400);
            }

            return Result<Unit>.Success(Unit.Value, "Genre deleted successfully");
        }
    }
}
