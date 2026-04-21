using CaloriePal.Application.Interfaces;
using CaloriePal.Application.Streaks.PurchaseStreakFreeze;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace caloriepal_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShopController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        public ShopController(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpGet("purchase-streak-freeze")]
        public async Task<IActionResult> PurchaseStreakFreeze(CancellationToken ct)
        {
            var result = await _mediator.Send(new PurchaseStreakFreezeCommand(_currentUser.UserId), ct);
            return Ok(result);
        }
    }
}