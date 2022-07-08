using Microsoft.AspNetCore.Mvc;

namespace ReferenceArchitecture.Application.Controllers;

[ApiController]
[Route("/api/v1/demo/stuff")]
public class DemoController : Controller
{
    // GET
    [HttpGet]
    public IActionResult Index()
    {
        return Json(new
        {
            hi = "there"
        });
    }
}