using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AwingController : ControllerBase
{
    private readonly IAwingService _awingService;
    public AwingController(IAwingService awingService)
    {
        _awingService = awingService;
    }
    
    [HttpPost("calculate-fuel")]
    public IActionResult Get([FromBody] TreasureRequest treasureRequest)
    {
        var result = _awingService.CalculateFuel(treasureRequest);
        return Ok(result);
    }
}