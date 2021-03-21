using System.Security.Claims;

namespace TodoFlutter.data.Infrastructure
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}
