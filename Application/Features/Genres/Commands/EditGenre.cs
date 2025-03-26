using Application.Core;
using Application.Features.Genres.DTOs;
using Application.Repositories.GenreRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Genres.Commands;

public class EditGenre
{
    public class Command : IRequest<Result<Unit>>
    {
        public required EditGenreDto GenreDto { get; set; }
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
            var genre = await _genreReadRepository.GetByIdAsync(request.GenreDto.Id);

            if (genre is null)
            {
                return Result<Unit>.Failure("Genre not found", 404);
            }

            var duplicate = await _genreReadRepository
                .GetWhere(g => g.Name.ToLower() == request.GenreDto.Name.ToLower() && g.Id != request.GenreDto.Id && !g.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (duplicate is not null)
            {
                return Result<Unit>.Failure("Another genre with the same name already exists", 400);
            }

            genre.Name = request.GenreDto.Name;

            var result = await _genreWriteRepository.SaveAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to update genre", 400);
            }

            return Result<Unit>.Success(Unit.Value, "Genre updated successfully");
        }
    }
}
