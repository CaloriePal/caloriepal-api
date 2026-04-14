using System.Security.Claims;
using CaloriePal.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CaloriePal.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        public Guid? UserId
        {
            get
            {
                var value = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? httpContextAccessor.HttpContext?.User.FindFirstValue("sub");

                return Guid.TryParse(value, out var id) ? id : null;
            }
        }

        public bool IsAuthenticated =>
            httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }
}
