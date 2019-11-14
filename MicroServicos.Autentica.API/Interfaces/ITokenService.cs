using MicroServicos.Autentica.API.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace MicroServicos.Autentica.API.Interfaces
{
    public interface ITokenService
    {
        TokenViewModel GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}