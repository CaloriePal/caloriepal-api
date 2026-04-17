using System.Security.Claims;
using CaloriePal.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CaloriePal.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        public Guid UserId
        {
            get
            {
                var value = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? httpContextAccessor.HttpContext?.User.FindFirstValue("sub");
                return Guid.Parse(value!);
            }
        }

        public bool IsAuthenticated =>
            httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }
}