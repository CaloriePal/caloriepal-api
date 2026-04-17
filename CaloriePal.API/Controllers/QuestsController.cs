using CaloriePal.Application.Interfaces;
using CaloriePal.Application.Quests.CompleteQuest;
using CaloriePal.Application.Quests.GetDailyQuests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace caloriepal_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuestsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        public QuestsController(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpGet("daily")]
        [ProducesResponseType(typeof(DailyQuestsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDaily(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetDailyQuestsQuery(_currentUser.UserId), ct);
            return Ok(result);
        }

        [HttpPost("{questId:guid}/complete")]
        [ProducesResponseType(typeof(CompleteQuestResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Complete(Guid questId, CancellationToken ct)
        {
            var result = await _mediator.Send(new CompleteQuestCommand(_currentUser.UserId, questId), ct);
            return Ok(result);
        }
    }
}