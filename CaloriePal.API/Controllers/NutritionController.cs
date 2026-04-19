using CaloriePal.Application.Interfaces;
using CaloriePal.Application.Nutrition.GetDailyNutrition;
using CaloriePal.Application.Nutrition.LogMeal;
using CaloriePal.Application.Nutrition.SearchFoodItems;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace caloriepal_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NutritionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        public NutritionController(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDaily([FromQuery] DateOnly? date)
        {
            var result = await _mediator.Send(new GetDailyNutritionQuery(_currentUser.UserId, date ?? DateOnly.FromDateTime(DateTime.UtcNow)));
            return Ok(result);
        }

        [HttpPost("meals")]
        public async Task<IActionResult> LogMeal([FromBody] LogMealRequest req)
        {
            var result = await _mediator.Send(new LogMealCommand(_currentUser.UserId, req.Category, req.FoodItemId, req.QuantityGrams, req.ManualName, req.ManualCalories, req.ManualProtein, req.ManualCarbs, req.ManualFat));
            return Ok(result);
        }

        [HttpGet("foods/search")]
        public async Task<IActionResult> SearchFoods([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return Ok(Array.Empty<FoodItemDto>());

            var result = await _mediator.Send(new SearchFoodItemsQuery(term, _currentUser.UserId));
            return Ok(result);
        }
    }
}