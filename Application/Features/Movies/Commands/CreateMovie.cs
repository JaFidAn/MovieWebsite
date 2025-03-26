using Application.Core;
using Application.Features.Movies.DTOs;
using Application.Repositories.MovieRepository;
using Application.Repositories.GenreRepository;
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
        private readonly IMapper _mapper;

        public Handler(
            IMovieWriteRepository movieWriteRepository,
            IGenreReadRepository genreReadRepository,
            IMapper mapper)
        {
            _movieWriteRepository = movieWriteRepository;
            _genreReadRepository = genreReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var genres = await _genreReadRepository
                .GetWhere(g => request.MovieDto.GenreIds.Contains(g.Id) && !g.IsDeleted)
                .ToListAsync(cancellationToken);

            if (genres.Count != request.MovieDto.GenreIds.Count)
            {
                return Result<string>.Failure("One or more genres are invalid", 400);
            }

            var movie = _mapper.Map<Movie>(request.MovieDto);

            movie.MovieGenres = request.MovieDto.GenreIds
                .Select(id => new MovieGenre
                {
                    GenreId = id,
                    MovieId = movie.Id
                })
                .ToList();

            await _movieWriteRepository.AddAsync(movie);
            var result = await _movieWriteRepository.SaveAsync() > 0;

            if (!result)
            {
                return Result<string>.Failure("Movie could not be created", 400);
            }

            return Result<string>.Success(movie.Id, "Movie added successfully");
        }
    }
}
