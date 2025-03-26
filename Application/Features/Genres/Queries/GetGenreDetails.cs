using Application.Core;
using Application.Features.Genres.DTOs;
using Application.Repositories.GenreRepository;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Genres.Queries;

public class GetGenreDetails
{
    public class Query : IRequest<Result<GenreDto>>
    {
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<GenreDto>>
    {
        private readonly IGenreReadRepository _genreReadRepository;
        private readonly IMapper _mapper;

        public Handler(IGenreReadRepository genreReadRepository, IMapper mapper)
        {
            _genreReadRepository = genreReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<GenreDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var genre = await _genreReadRepository
                .GetWhere(g => !g.IsDeleted)
                .ProjectTo<GenreDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (genre is null)
            {
                return Result<GenreDto>.Failure("Genre not found", 404);
            }

            return Result<GenreDto>.Success(genre);
        }
    }
}
