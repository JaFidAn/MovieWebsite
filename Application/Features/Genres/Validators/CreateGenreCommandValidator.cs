using FluentValidation;
using Application.Features.Genres.Commands;

namespace Application.Features.Genres.Validators;

public class CreateGenreCommandValidator : AbstractValidator<CreateGenre.Command>
{
    public CreateGenreCommandValidator()
    {
        RuleFor(x => x.GenreDto.Name)
            .NotEmpty().WithMessage("Genre name is required.")
            .MaximumLength(100).WithMessage("Genre name must be at most 100 characters.");
    }
}
