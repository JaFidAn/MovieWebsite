using Application.Core;
using Application.Features.Directors.Commands;
using Application.Features.Directors.DTOs;
using Application.Features.Directors.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class DirectorsController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PagedResult<DirectorDto>>> GetAll([FromQuery] PaginationParams paginationParams)
    {
        return await Mediator.Send(new GetDirectorList.Query { Params = paginationParams });
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<DirectorDto>> GetDetail(string id)
    {
        return HandleResult(await Mediator.Send(new GetDirectorDetails.Query { Id = id }));
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create(CreateDirectorDto directorDto)
    {
        return HandleResult(await Mediator.Send(new CreateDirector.Command { DirectorDto = directorDto }));
    }

    [HttpPut]
    public async Task<ActionResult> Edit(EditDirectorDto directorDto)
    {
        return HandleResult(await Mediator.Send(new EditDirector.Command { DirectorDto = directorDto }));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteDirector.Command { Id = id }));
    }
}
