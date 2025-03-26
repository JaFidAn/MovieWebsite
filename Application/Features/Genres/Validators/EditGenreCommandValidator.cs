using FluentValidation;
using Application.Features.Genres.Commands;

namespace Application.Features.Genres.Validators;

public class EditGenreCommandValidator : AbstractValidator<EditGenre.Command>
{
    public EditGenreCommandValidator()
    {
        RuleFor(x => x.GenreDto.Id)
            .NotEmpty().WithMessage("Genre ID is required.");

        RuleFor(x => x.GenreDto.Name)
            .NotEmpty().WithMessage("Genre name is required.")
            .MaximumLength(100).WithMessage("Genre name must be at most 100 characters.");
    }
}
