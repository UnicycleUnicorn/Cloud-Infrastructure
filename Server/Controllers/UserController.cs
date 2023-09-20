using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Mvc;
using Server.Security;

namespace Server.Controllers;

// Authorize Register Logout Unregister ChangePassword Refresh


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    public static readonly AbstractTokenStore TokenStore = new NaiveTokenStore(TimeSpan.FromSeconds(8), TimeSpan.FromSeconds(40), TimeSpan.FromMinutes(5));

    [HttpPost("Authorize", Name = "Authorize")]
    public IActionResult Authorize([FromBody] UserIdentification userIdentification)
    {
        // Validate user credentials (e.g., check against a database)
        if (Users.ValidateAndGetRoles(userIdentification.Username, userIdentification.Password, out string[]? roles))
        {
            var (authorizationToken, refreshToken) = TokenStore.GenerateTokenSet(userIdentification.Username, roles);

            // Return the token as a response
            return Ok(new DualToken(authorizationToken, refreshToken));
        }

        // If credentials are invalid, return a 403 Forbid response
        return Forbid();
    }

    [HttpPost("Refresh", Name = "Refresh")]
    public IActionResult Refresh([FromBody] SingleToken refreshToken)
    {
        if (TokenStore.RemoveAndVerifyRefreshToken(refreshToken.Token, out string? username, out string? newRefreshToken))
        {
            if (Users.GetRoles(username, out string[]? roles))
            {
                string newAuthorizationToken =  TokenStore.GenerateAuthorizationToken(username, roles);
                return Ok(new DualToken(newAuthorizationToken, newRefreshToken));
            } else
            {
                // If credentials are invalid, return a 403 Forbid response
                return Forbid();
            }
        }

        return Unauthorized();
    }
}
