using Application.Core;
using Application.Repositories.MovieRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Movies.Commands;

public class DeleteMovie
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IMovieReadRepository _movieReadRepository;
        private readonly IMovieWriteRepository _movieWriteRepository;

        public Handler(
            IMovieReadRepository movieReadRepository,
            IMovieWriteRepository movieWriteRepository)
        {
            _movieReadRepository = movieReadRepository;
            _movieWriteRepository = movieWriteRepository;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var movie = await _movieReadRepository
                .GetWhere(m => m.Id == request.Id && !m.IsDeleted)
                .Include(m => m.MovieGenres)
                .FirstOrDefaultAsync(cancellationToken);

            if (movie is null)
            {
                return Result<Unit>.Failure("Movie not found", 404);
            }

            _movieWriteRepository.RemoveMovieGenres(movie);

            movie.IsDeleted = true;

            var result = await _movieWriteRepository.SaveAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to delete movie", 400);
            }

            return Result<Unit>.Success(Unit.Value, "Movie deleted successfully");
        }
    }
}
