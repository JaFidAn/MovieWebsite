using FluentValidation;
using Application.Features.Auth.Commands;

namespace Application.Features.Auth.Validators;

public class LoginCommandValidator : AbstractValidator<Login.Command>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.LoginDto.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.");

        RuleFor(x => x.LoginDto.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
