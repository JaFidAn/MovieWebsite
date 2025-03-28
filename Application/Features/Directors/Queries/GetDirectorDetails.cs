using Application.Core;
using Application.Features.Directors.DTOs;
using Application.Repositories.DirectorRepository;
using Application.Utilities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Directors.Queries;

public class GetDirectorDetails
{
    public class Query : IRequest<Result<DirectorDto>>
    {
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<DirectorDto>>
    {
        private readonly IDirectorReadRepository _directorReadRepository;
        private readonly IMapper _mapper;

        public Handler(IDirectorReadRepository directorReadRepository, IMapper mapper)
        {
            _directorReadRepository = directorReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<DirectorDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var director = await _directorReadRepository
                .GetWhere(d => !d.IsDeleted)
                .ProjectTo<DirectorDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (director is null)
            {
                return Result<DirectorDto>.Failure(MessageGenerator.NotFound("Director"), 404);
            }

            return Result<DirectorDto>.Success(director);
        }
    }
}
