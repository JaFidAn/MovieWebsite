using Application.Core;
using Application.Features.Movies.DTOs;
using Application.Repositories.MovieRepository;
using Application.Repositories.GenreRepository;
using Application.Repositories.ActorRepository;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Movies.Commands;

public class CreateMovie
{
    public class Command : IRequest<Result<string>>
    {
        public CreateMovieDto MovieDto { get; set; } = null!;
    }

    public class Handler : IRequestHandler<Command, Result<string>>
    {
        private readonly IMovieWriteRepository _movieWriteRepository;
        private readonly IGenreReadRepository _genreReadRepository;
        private readonly IActorReadRepository _actorReadRepository;
        private readonly IMapper _mapper;

        public Handler(
            IMovieWriteRepository movieWriteRepository,
            IGenreReadRepository genreReadRepository,
            IActorReadRepository actorReadRepository,
            IMapper mapper)
        {
            _movieWriteRepository = movieWriteRepository;
            _genreReadRepository = genreReadRepository;
            _actorReadRepository = actorReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var genreList = await _genreReadRepository
                .GetWhere(g => request.MovieDto.GenreIds.Contains(g.Id) && !g.IsDeleted)
                .ToListAsync(cancellationToken);

            if (genreList.Count != request.MovieDto.GenreIds.Count)
                return Result<string>.Failure("One or more genres are invalid", 400);

            var actorList = await _actorReadRepository
                .GetWhere(a => request.MovieDto.ActorIds.Contains(a.Id) && !a.IsDeleted)
                .ToListAsync(cancellationToken);

            if (actorList.Count != request.MovieDto.ActorIds.Count)
                return Result<string>.Failure("One or more actors are invalid", 400);

            var movie = _mapper.Map<Movie>(request.MovieDto);
            movie.DirectorId = request.MovieDto.DirectorId;

            movie.MovieGenres = request.MovieDto.GenreIds
                .Select(id => new MovieGenre { GenreId = id, MovieId = movie.Id })
                .ToList();

            movie.MovieActors = request.MovieDto.ActorIds
                .Select(id => new MovieActor { ActorId = id, MovieId = movie.Id })
                .ToList();

            await _movieWriteRepository.AddAsync(movie);
            var result = await _movieWriteRepository.SaveAsync() > 0;

            if (!result)
                return Result<string>.Failure("Movie could not be created", 400);

            return Result<string>.Success(movie.Id, "Movie added successfully");
        }
    }
}
