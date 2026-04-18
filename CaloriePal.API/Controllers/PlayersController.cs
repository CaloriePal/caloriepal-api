using CaloriePal.Application.Interfaces;
using CaloriePal.Application.Players.GetActivityLog;
using CaloriePal.Application.Players.GetPlayerStats;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace caloriepal_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlayersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        public PlayersController(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpGet("me/stats")]
        [ProducesResponseType(typeof(PlayerStatsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyStats(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPlayerStatsQuery(_currentUser.UserId), ct);
            return Ok(result);
        }

        [HttpGet("me/activity-log")]
        public async Task<IActionResult> GetActivityLog()
        {
            var result = await _mediator.Send(new GetActivityLogQuery(_currentUser.UserId));
            return Ok(result);
        }
    }
}