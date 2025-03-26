using FluentValidation;
using Application.Features.Movies.Commands;

namespace Application.Features.Movies.Validators;

public class EditMovieCommandValidator : AbstractValidator<EditMovie.Command>
{
    public EditMovieCommandValidator()
    {
        RuleFor(x => x.MovieDto.Id)
            .NotEmpty().WithMessage("Movie ID is required.");

        RuleFor(x => x.MovieDto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must be at most 200 characters.");

        RuleFor(x => x.MovieDto.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must be at most 1000 characters.");

        RuleFor(x => x.MovieDto.ReleaseYear)
            .GreaterThan(1800).WithMessage("Release year must be greater than 1800.");

        RuleFor(x => x.MovieDto.Rating)
            .InclusiveBetween(0.0, 10.0).WithMessage("Rating must be between 0 and 10.");

        RuleFor(x => x.MovieDto.GenreIds)
            .NotEmpty().WithMessage("At least one genre must be selected.");
    }
}
