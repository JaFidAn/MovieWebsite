using FluentValidation;
using Application.Features.Actors.Commands;

namespace Application.Features.Actors.Validators;

public class EditActorCommandValidator : AbstractValidator<EditActor.Command>
{
    public EditActorCommandValidator()
    {
        RuleFor(x => x.ActorDto.Id)
            .NotEmpty().WithMessage("Actor ID is required.");

        RuleFor(x => x.ActorDto.FullName)
            .NotEmpty().WithMessage("Actor name is required.")
            .MaximumLength(150).WithMessage("Actor name must be at most 150 characters.");
    }
}
