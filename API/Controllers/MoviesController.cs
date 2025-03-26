using Application.Core;
using Application.Features.Movies.Commands;
using Application.Features.Movies.DTOs;
using Application.Features.Movies.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MoviesController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<MovieDto>>> GetAll([FromQuery] PaginationParams paginationParams)
    {
        return await Mediator.Send(new GetMovieList.Query { Params = paginationParams });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetDetail(string id)
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
}
