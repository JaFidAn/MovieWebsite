using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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
                // Not Found
                if (result.Code == 404)
                    return NotFound(new { message = result.Error, code = result.Code });

                // General Bad Request
                return BadRequest(new { message = result.Error, code = result.Code });
            }

            var type = typeof(T);

            // Success (Unit) with message
            if (type == typeof(Unit))
                return Ok(new { message = result.Message });

            // Success with simple types (id)
            if (type == typeof(string) || type == typeof(Guid) || type == typeof(int))
                return Ok(new { id = result.Value, message = result.Message });

            // Success with DTOs (GET requests)
            if (result.Message == null)
            {
                return Ok(result.Value); // Just return the object directly
            }

            // Otherwise return data and message
            return Ok(new
            {
                data = result.Value,
                message = result.Message
            });
        }

    }
}
