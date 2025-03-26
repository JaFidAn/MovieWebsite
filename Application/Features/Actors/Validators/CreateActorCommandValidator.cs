using FluentValidation;
using Application.Features.Actors.Commands;

namespace Application.Features.Actors.Validators;

public class CreateActorCommandValidator : AbstractValidator<CreateActor.Command>
{
    public CreateActorCommandValidator()
    {
        RuleFor(x => x.ActorDto.FullName)
            .NotEmpty().WithMessage("Actor name is required.")
            .MaximumLength(150).WithMessage("Actor name must be at most 150 characters.");
    }
}
