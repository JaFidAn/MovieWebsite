using Application.Core;
using Application.Features.Movies.DTOs;
using Application.Repositories.MovieRepository;
using Application.Repositories.GenreRepository;
using Application.Repositories.ActorRepository;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Utilities;

namespace Application.Features.Movies.Commands;

public class EditMovie
{
    public class Command : IRequest<Result<Unit>>
    {
        public required EditMovieDto MovieDto { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IMovieReadRepository _movieReadRepository;
        private readonly IMovieWriteRepository _movieWriteRepository;
        private readonly IGenreReadRepository _genreReadRepository;
        private readonly IActorReadRepository _actorReadRepository;
        private readonly IMapper _mapper;

        public Handler(
            IMovieReadRepository movieReadRepository,
            IMovieWriteRepository movieWriteRepository,
            IGenreReadRepository genreReadRepository,
            IActorReadRepository actorReadRepository,
            IMapper mapper)
        {
            _movieReadRepository = movieReadRepository;
            _movieWriteRepository = movieWriteRepository;
            _genreReadRepository = genreReadRepository;
            _actorReadRepository = actorReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var movie = await _movieReadRepository
                .GetWhere(m => m.Id == request.MovieDto.Id && !m.IsDeleted)
                .Include(m => m.MovieGenres)
                .Include(m => m.MovieActors)
                .FirstOrDefaultAsync(cancellationToken);

            if (movie is null)
            {
                return Result<Unit>.Failure(MessageGenerator.NotFound("Movie"), 404);
            }

            var genres = await _genreReadRepository
                .GetWhere(g => request.MovieDto.GenreIds.Contains(g.Id) && !g.IsDeleted)
                .ToListAsync(cancellationToken);

            if (genres.Count != request.MovieDto.GenreIds.Count)
            {
                return Result<Unit>.Failure(MessageGenerator.InvalidEntities("genre"), 400);
            }

            var actors = await _actorReadRepository
                .GetWhere(a => request.MovieDto.ActorIds.Contains(a.Id) && !a.IsDeleted)
                .ToListAsync(cancellationToken);

            if (actors.Count != request.MovieDto.ActorIds.Count)
            {
                return Result<Unit>.Failure(MessageGenerator.InvalidEntities("actor"), 400);
            }

            _movieWriteRepository.RemoveMovieGenres(movie);
            _movieWriteRepository.RemoveMovieActors(movie);

            _mapper.Map(request.MovieDto, movie);

            movie.MovieGenres = request.MovieDto.GenreIds
                .Select(id => new MovieGenre { MovieId = movie.Id, GenreId = id })
                .ToList();

            movie.MovieActors = request.MovieDto.ActorIds
                .Select(id => new MovieActor { MovieId = movie.Id, ActorId = id })
                .ToList();

            var result = await _movieWriteRepository.SaveAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure(MessageGenerator.UpdateFailed("movie"), 400);
            }

            return Result<Unit>.Success(Unit.Value, MessageGenerator.UpdateSuccess("Movie"));
        }
    }
}
