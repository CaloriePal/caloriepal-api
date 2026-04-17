using CaloriePal.Application.Auth.SyncProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaloriePal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [Authorize]
        [HttpPost("sync-profile")]
        [ProducesResponseType(typeof(SyncProfileResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> SyncProfile([FromBody] SyncProfileRequest request, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new SyncProfileCommand(request.DisplayName, request.AvatarUrl),
                cancellationToken);

            return Ok(result);
        }
    }

    public record SyncProfileRequest(string DisplayName, string? AvatarUrl);
}