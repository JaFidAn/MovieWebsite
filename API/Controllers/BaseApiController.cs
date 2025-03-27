using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private IMediator? _mediator;

        protected IMediator Mediator => _mediator
             ??= HttpContext.RequestServices.GetService<IMediator>()
             ?? throw new InvalidOperationException("IMediator service is unavailable");

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (!result.IsSuccess)
            {
                if (result.Code == 404)
                    return NotFound(new { message = result.Error, code = result.Code });

                return BadRequest(new { message = result.Error, code = result.Code });
            }

            var type = typeof(T);

            if (type == typeof(Unit))
                return Ok(new { message = result.Message });

            if (type == typeof(string) || type == typeof(Guid) || type == typeof(int))
            {
                var actionName = ControllerContext.ActionDescriptor.ActionName?.ToLower();

                var key = actionName switch
                {
                    "login" => "token",
                    "register" => "token",
                    _ => "id"
                };

                dynamic obj = new ExpandoObject();
                ((IDictionary<string, object>)obj)[key] = result.Value!;
                obj.message = result.Message;

                return Ok(obj);
            }

            if (result.Message == null)
            {
                return Ok(result.Value);
            }

            return Ok(new
            {
                data = result.Value,
                message = result.Message
            });
        }
    }
}
