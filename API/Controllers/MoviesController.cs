using Application.Core;
using Application.Features.Movies.Commands;
using Application.Features.Movies.DTOs;
using Application.Features.Movies.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MoviesController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PagedResult<MovieDto>>> GetAll([FromQuery] PaginationParams paginationParams)
    {
        return await Mediator.Send(new GetMovieList.Query { Params = paginationParams });
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDetailsDto>> GetDetail(string id)
    {
        return HandleResult(await Mediator.Send(new GetMovieDetails.Query { Id = id }));
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create(CreateMovieDto movieDto)
    {
        return HandleResult(await Mediator.Send(new CreateMovie.Command { MovieDto = movieDto }));
    }

    [HttpPut]
    public async Task<ActionResult> Edit(EditMovieDto movieDto)
    {
        return HandleResult(await Mediator.Send(new EditMovie.Command { MovieDto = movieDto }));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteMovie.Command { Id = id }));
    }

    [HttpGet("search")]
    public async Task<ActionResult<PagedResult<MovieSearchResultDto>>> Search([FromQuery] MovieSearchParams queryParams)
    {
        return await Mediator.Send(new GetMovieSearchResults.Query { Params = queryParams });
    }

    [AllowAnonymous]
    [HttpGet("external")]
    public async Task<ActionResult<List<ExternalMovieDto>>> GetExternalMovies()
    {
        return HandleResult(await Mediator.Send(new GetExternalMovies.Query()));
    }
}
