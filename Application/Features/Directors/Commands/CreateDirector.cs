using Application.Core;
using Application.Features.Directors.DTOs;
using Application.Repositories.DirectorRepository;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Directors.Commands;

public class CreateDirector
{
    public class Command : IRequest<Result<string>>
    {
        public CreateDirectorDto DirectorDto { get; set; } = null!;
    }

    public class Handler : IRequestHandler<Command, Result<string>>
    {
        private readonly IDirectorWriteRepository _directorWriteRepository;
        private readonly IDirectorReadRepository _directorReadRepository;
        private readonly IMapper _mapper;

        public Handler(
            IDirectorWriteRepository directorWriteRepository,
            IDirectorReadRepository directorReadRepository,
            IMapper mapper)
        {
            _directorWriteRepository = directorWriteRepository;
            _directorReadRepository = directorReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var existing = await _directorReadRepository
                .GetWhere(d => d.FullName.ToLower() == request.DirectorDto.FullName.ToLower() && !d.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (existing is not null)
            {
                return Result<string>.Failure("Director with the same name already exists", 400);
            }

            var director = _mapper.Map<Director>(request.DirectorDto);

            await _directorWriteRepository.AddAsync(director);
            var result = await _directorWriteRepository.SaveAsync() > 0;

            if (!result)
            {
                return Result<string>.Failure("Director could not be created", 400);
            }

            return Result<string>.Success(director.Id, "Director added successfully");
        }
    }
}
