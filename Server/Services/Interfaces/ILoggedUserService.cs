using System.Security.Claims;

namespace messanger.Server.Services.Interfaces
{
    public interface ILoggedUserService
    {
        ClaimsPrincipal Claims { get; }
        string Id { get; }
    }
}
