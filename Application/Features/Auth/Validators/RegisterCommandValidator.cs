using FluentValidation;
using Application.Features.Auth.Commands;

namespace Application.Features.Auth.Validators;

public class RegisterCommandValidator : AbstractValidator<Register.Command>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.RegisterDto.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(150).WithMessage("Full name must be at most 150 characters.");

        RuleFor(x => x.RegisterDto.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.RegisterDto.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}
