using System.Security.Claims;

namespace TaskTracker.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, string userName, string firstName, string lastName, string role, IEnumerable<Claim> additionalClaims = null);
        string RefreshToken(string token);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
