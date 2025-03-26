using Application.Core;
using Application.Features.Genres.DTOs;
using Application.Repositories.GenreRepository;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Genres.Commands;

public class CreateGenre
{
    public class Command : IRequest<Result<string>>
    {
        public CreateGenreDto GenreDto { get; set; } = null!;
    }

    public class Handler : IRequestHandler<Command, Result<string>>
    {
        private readonly IGenreWriteRepository _genreWriteRepository;
        private readonly IGenreReadRepository _genreReadRepository;
        private readonly IMapper _mapper;

        public Handler(
            IGenreWriteRepository genreWriteRepository,
            IGenreReadRepository genreReadRepository,
            IMapper mapper)
        {
            _genreWriteRepository = genreWriteRepository;
            _genreReadRepository = genreReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var existing = await _genreReadRepository
                .GetWhere(g => g.Name.ToLower() == request.GenreDto.Name.ToLower() && !g.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (existing is not null)
            {
                return Result<string>.Failure("Genre with the same name already exists", 400);
            }

            var genre = _mapper.Map<Genre>(request.GenreDto);

            await _genreWriteRepository.AddAsync(genre);
            var result = await _genreWriteRepository.SaveAsync() > 0;

            if (!result)
            {
                return Result<string>.Failure("Genre could not be created", 400);
            }

            return Result<string>.Success(genre.Id, "Genre added successfully");
        }
    }
}
