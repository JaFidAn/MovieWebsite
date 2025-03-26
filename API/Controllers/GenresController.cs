using Application.Core;
using Application.Features.Genres.Commands;
using Application.Features.Genres.DTOs;
using Application.Features.Genres.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class GenresController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<GenreDto>>> GetAll([FromQuery] PaginationParams paginationParams)
    {
        return await Mediator.Send(new GetGenreList.Query { Params = paginationParams });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GenreDto>> GetDetail(string id)
    {
        return HandleResult(await Mediator.Send(new GetGenreDetails.Query { Id = id }));
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create(CreateGenreDto genreDto)
    {
        return HandleResult(await Mediator.Send(new CreateGenre.Command { GenreDto = genreDto }));
    }

    [HttpPut]
    public async Task<ActionResult> Edit(EditGenreDto genreDto)
    {
        return HandleResult(await Mediator.Send(new EditGenre.Command { GenreDto = genreDto }));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteGenre.Command { Id = id }));
    }
}
