using Application.Core;
using Application.Features.Auth.DTOs;
using Application.Services;
using Application.Utilities;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands;

public class Login
{
    public class Command : IRequest<Result<string>>
    {
        public LoginDto LoginDto { get; set; } = null!;
    }

    public class Handler : IRequestHandler<Command, Result<string>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public Handler(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.LoginDto.Email);

            if (user is null)
                return Result<string>.Failure(MessageGenerator.InvalidCredentials(), 401);

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.LoginDto.Password, false);

            if (!result.Succeeded)
                return Result<string>.Failure(MessageGenerator.InvalidCredentials(), 401);

            var token = await _tokenService.CreateToken(user);

            return Result<string>.Success(token, MessageGenerator.LoginSuccess());
        }
    }
}
