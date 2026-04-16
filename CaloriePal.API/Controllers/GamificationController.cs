using CaloriePal.Application.Interfaces;
using CaloriePal.Application.Streaks.GrantStreakFreeze;
using CaloriePal.Application.Streaks.UpdateStreak;
using CaloriePal.Application.XpEvents.AddXp;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaloriePal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GamificationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        public GamificationController(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpPost("xp")]
        [ProducesResponseType(typeof(AddXpResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddXp([FromBody] AddXpRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(
                new AddXpCommand(_currentUser.UserId!, request.Amount, request.Source), ct);
            return Ok(result);
        }

        [HttpPost("streak")]
        [ProducesResponseType(typeof(UpdateStreakResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateStreak(CancellationToken ct)
        {
            var result = await _mediator.Send(new UpdateStreakCommand(_currentUser.UserId!), ct);
            return Ok(result);
        }

        [HttpPost("streak/freeze")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> GrantFreeze([FromBody] GrantFreezeRequest request, CancellationToken ct)
        {
            var remaining = await _mediator.Send(
                new GrantStreakFreezeCommand(_currentUser.UserId!, request.Count), ct);
            return Ok(new { streakFreezes = remaining });
        }
    }

    public sealed record AddXpRequest(int Amount, string Source, string? Notes = null);
    public sealed record GrantFreezeRequest(int Count = 1);
}