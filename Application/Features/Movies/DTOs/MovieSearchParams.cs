using Application.Core;

namespace Application.Features.Movies.DTOs;

public class MovieSearchParams : PaginationParams
{
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public double? MinRating { get; set; }
}
