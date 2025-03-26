using Application.Features.Genres.DTOs;
using Application.Features.Movies.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Genre, GenreDto>().ReverseMap();
        CreateMap<Genre, CreateGenreDto>().ReverseMap();
        CreateMap<Genre, EditGenreDto>().ReverseMap();

        CreateMap<Movie, CreateMovieDto>().ReverseMap();
        CreateMap<Movie, EditMovieDto>().ReverseMap();
        CreateMap<Movie, MovieDto>()
            .ForMember(dest => dest.Genres,
                       opt => opt.MapFrom(src => src.MovieGenres.Select(mg => mg.Genre.Name)))
            .ReverseMap();
    }
}
