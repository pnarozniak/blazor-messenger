using System.Security.Claims;
using messanger.Server.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace messanger.Server.Services.Implementations
{
    public class LoggedUserService : ILoggedUserService
    {
        private readonly HttpContext _httpContext;
        public LoggedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public ClaimsPrincipal LoggedUser => _httpContext.User;
    }
}
