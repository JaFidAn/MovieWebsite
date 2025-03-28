using Application.Features.Movies.DTOs;
using Application.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Services;

public class ExternalMovieService : IExternalMovieService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ExternalMovieService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<ExternalMovieDto>> GetPopularMoviesAsync(int page = 1)
    {
        var apiKey = _configuration["Tmdb:ApiKey"];
        var url = $"https://api.themoviedb.org/3/movie/popular?api_key={apiKey}&language=en-US&page={page}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Cache-Control", "no-cache");

        var responseMessage = await _httpClient.SendAsync(request);
        responseMessage.EnsureSuccessStatusCode();

        var response = await responseMessage.Content.ReadFromJsonAsync<TmdbResponse>();

        return response?.Results
            .Where(m => !string.IsNullOrWhiteSpace(m.ReleaseDate))
            .Select(m =>
            {
                int year = 0;
                if (DateTime.TryParse(m.ReleaseDate, out var parsedDate))
                {
                    year = parsedDate.Year;
                }

                return new ExternalMovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    ReleaseYear = year,
                    Rating = m.VoteAverage ?? 0
                };
            })
            .Where(m => m.ReleaseYear > 0 && m.Rating > 0)
            .ToList() ?? new();
    }


    private class TmdbResponse
    {
        public List<TmdbMovie> Results { get; set; } = new();
    }

    private class TmdbMovie
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("vote_average")]
        public double? VoteAverage { get; set; }
    }
}
