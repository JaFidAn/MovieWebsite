using Application.Core;
using Application.Features.Auth.DTOs;
using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands;

public class Register
{
    public class Command : IRequest<Result<string>>
    {
        public RegisterDto RegisterDto { get; set; } = null!;
    }

    public class Handler : IRequestHandler<Command, Result<string>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public Handler(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = new AppUser
            {
                UserName = request.RegisterDto.Email,
                Email = request.RegisterDto.Email,
                FullName = request.RegisterDto.FullName
            };

            var result = await _userManager.CreateAsync(user, request.RegisterDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<string>.Failure($"Registration failed: {errors}", 400);
            }

            var token = await _tokenService.CreateToken(user);

            return Result<string>.Success(token, "Registration successful");
        }
    }
}
