using Application.Core;
using Application.Features.Actors.Commands;
using Application.Features.Actors.DTOs;
using Application.Features.Actors.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActorsController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PagedResult<ActorDto>>> GetAll([FromQuery] PaginationParams paginationParams)
    {
        return await Mediator.Send(new GetActorList.Query { Params = paginationParams });
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<ActorDto>> GetDetail(string id)
    {
        return HandleResult(await Mediator.Send(new GetActorDetails.Query { Id = id }));
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create(CreateActorDto actorDto)
    {
        return HandleResult(await Mediator.Send(new CreateActor.Command { ActorDto = actorDto }));
    }

    [HttpPut]
    public async Task<ActionResult> Edit(EditActorDto actorDto)
    {
        return HandleResult(await Mediator.Send(new EditActor.Command { ActorDto = actorDto }));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteActor.Command { Id = id }));
    }
}
