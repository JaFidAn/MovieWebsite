using Application.Core;
using Application.Features.Movies.DTOs;
using Application.Repositories.MovieRepository;
using Application.Repositories.GenreRepository;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        private readonly IMapper _mapper;

        public Handler(
            IMovieReadRepository movieReadRepository,
            IMovieWriteRepository movieWriteRepository,
            IGenreReadRepository genreReadRepository,
            IMapper mapper)
        {
            _movieReadRepository = movieReadRepository;
            _movieWriteRepository = movieWriteRepository;
            _genreReadRepository = genreReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var movie = await _movieReadRepository
                .GetWhere(m => m.Id == request.MovieDto.Id && !m.IsDeleted)
                .Include(m => m.MovieGenres)
                .FirstOrDefaultAsync(cancellationToken);

            if (movie is null)
            {
                return Result<Unit>.Failure("Movie not found", 404);
            }

            var genres = await _genreReadRepository
                .GetWhere(g => request.MovieDto.GenreIds.Contains(g.Id) && !g.IsDeleted)
                .ToListAsync(cancellationToken);

            if (genres.Count != request.MovieDto.GenreIds.Count)
            {
                return Result<Unit>.Failure("One or more genres are invalid", 400);
            }

            _movieWriteRepository.RemoveMovieGenres(movie);

            movie.MovieGenres = request.MovieDto.GenreIds
                .Select(genreId => new MovieGenre
                {
                    MovieId = movie.Id,
                    GenreId = genreId
                }).ToList();

            movie.Title = request.MovieDto.Title;
            movie.Description = request.MovieDto.Description;
            movie.ReleaseYear = request.MovieDto.ReleaseYear;
            movie.Rating = request.MovieDto.Rating;

            var result = await _movieWriteRepository.SaveAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to update movie", 400);
            }

            return Result<Unit>.Success(Unit.Value, "Movie updated successfully");
        }
    }
}
