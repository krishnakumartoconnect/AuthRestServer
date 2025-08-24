using Microsoft.AspNetCore.Mvc;

public class AuthController : Controller
{
    private readonly JwtTokenService _jwt;

    public AuthController(JwtTokenService jwt)
    {
        _jwt = jwt;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        // Simple demo check
        if (username == "admin" && password == "password")
        {
            var token = _jwt.GenerateToken(username, "Admin");

            // Show or return token
            ViewBag.Token = token;
            return View("Token");
        }

        ModelState.AddModelError("", "Invalid credentials");
        return View();
    }
}
