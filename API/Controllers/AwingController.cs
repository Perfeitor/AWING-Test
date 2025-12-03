using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AwingController : ControllerBase
{
    [HttpGet("GetTest")]
    public IActionResult Get()
    {
        return Ok();
    }
}