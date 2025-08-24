using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class SecretsController : Controller
{
    private readonly SecretManagerService _secretService;

    public SecretsController(SecretManagerService secretService)
    {
        _secretService = secretService;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Store(string name, string value)
    {
        await _secretService.PutSecretAsync(name, value);
        ViewBag.Message = "Secret stored!";
        return View("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Get(string name)
    {
        var secret = await _secretService.GetSecretAsync(name);
        ViewBag.Secret = secret ?? "Secret not found";
        return View("Index");
    }
}
