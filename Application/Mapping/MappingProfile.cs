using Application.Features.Actors.DTOs;
using Application.Features.Directors.DTOs;
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
        CreateMap<EditMovieDto, Movie>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MovieGenres, opt => opt.Ignore())
            .ForMember(dest => dest.MovieActors, opt => opt.Ignore());
        CreateMap<Movie, MovieDto>()
            .ForMember(dest => dest.Genres,
                       opt => opt.MapFrom(src => src.MovieGenres.Select(mg => mg.Genre.Name)))
            .ReverseMap();

        CreateMap<Movie, MovieDetailsDto>()
            .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director.FullName))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.MovieGenres.Select(mg => mg.Genre.Name)))
            .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.MovieActors.Select(ma => ma.Actor.FullName)));

        CreateMap<Director, DirectorDto>().ReverseMap();
        CreateMap<Director, CreateDirectorDto>().ReverseMap();
        CreateMap<Director, EditDirectorDto>().ReverseMap();

        CreateMap<Actor, ActorDto>().ReverseMap();
        CreateMap<Actor, CreateActorDto>().ReverseMap();
        CreateMap<Actor, EditActorDto>().ReverseMap();
    }
}
