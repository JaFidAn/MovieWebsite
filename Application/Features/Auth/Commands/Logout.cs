using Application.Core;
using Application.Services;
using Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.Commands;

public class Logout
{
    public class Command : IRequest<Result<Unit>> { }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var context = _httpContextAccessor.HttpContext;

            if (context is null)
                return Result<Unit>.Failure("HttpContext is not available", 500);

            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrWhiteSpace(token))
                return Result<Unit>.Failure("Token is required", 400);

            await _tokenService.RevokeTokenAsync(token);

            return Result<Unit>.Success(Unit.Value, MessageGenerator.LogoutSuccess());
        }
    }
}
