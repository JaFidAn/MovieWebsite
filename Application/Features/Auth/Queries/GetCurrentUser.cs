using Application.Core;
using Application.Features.Auth.DTOs;
using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Features.Auth.Queries;

public class GetCurrentUser
{
    public class Query : IRequest<Result<UserDto>> { }

    public class Handler : IRequestHandler<Query, Result<UserDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<UserDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var email = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
                return Result<UserDto>.Failure("Unauthorized", 401);

            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Result<UserDto>.Failure("User not found", 404);

            var userDto = new UserDto
            {
                FullName = user.FullName,
                Email = user.Email!,
            };

            return Result<UserDto>.Success(userDto);
        }
    }
}
