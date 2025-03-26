using FluentValidation;
using Application.Features.Directors.Commands;

namespace Application.Features.Directors.Validators;

public class CreateDirectorCommandValidator : AbstractValidator<CreateDirector.Command>
{
    public CreateDirectorCommandValidator()
    {
        RuleFor(x => x.DirectorDto.FullName)
            .NotEmpty().WithMessage("Director name is required.")
            .MaximumLength(150).WithMessage("Director name must be at most 150 characters.");
    }
}
