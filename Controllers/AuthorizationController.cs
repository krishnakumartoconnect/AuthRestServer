using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

[ApiController]
public class AuthorizationController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthorizationController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("/connect/token"), IgnoreAntiforgeryToken]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();
#pragma warning disable CS8604 // Possible null reference argument.
        if (request.IsPasswordGrantType())
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Forbid();

            var claims = new List<Claim>
            {
                new Claim(OpenIddictConstants.Claims.Subject, user.Id),
                new Claim(OpenIddictConstants.Claims.Username, user.UserName)
            };

            var identity = new ClaimsIdentity(claims, TokenValidationParameters.DefaultAuthenticationType);
            var principal = new ClaimsPrincipal(identity);

            principal.SetScopes(new[] { OpenIddictConstants.Scopes.OpenId, OpenIddictConstants.Scopes.Profile, "api1" });
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
#pragma warning restore CS8604 // Possible null reference argument.

        return BadRequest();
    }
}
