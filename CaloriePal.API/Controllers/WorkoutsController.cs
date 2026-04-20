using CaloriePal.Application.Interfaces;
using CaloriePal.Application.Workouts.GetWorkoutStats;
using CaloriePal.Application.Workouts.LogWorkout;
using CaloriePal.Application.Workouts.SearchExercises;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaloriePal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkoutsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        public WorkoutsController(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _mediator.Send(new GetWorkoutStatsQuery(_currentUser.UserId));
            return Ok(result);
        }

        [HttpPost("sessions")]
        public async Task<IActionResult> LogWorkout([FromBody] LogWorkoutRequest req)
        {
            var result = await _mediator.Send(new LogWorkoutCommand(
                _currentUser.UserId,
                req.Name,
                req.Category,
                req.DurationMinutes,
                req.Exercises));
            return Ok(result);
        }

        [HttpGet("exercises/search")]
        public async Task<IActionResult> SearchExercises([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return Ok(Array.Empty<ExerciseDto>());

            var result = await _mediator.Send(new SearchExercisesQuery(term));
            return Ok(result);
        }
    }
}