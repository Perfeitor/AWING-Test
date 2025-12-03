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
    public async Task<IActionResult> Get([FromBody] TreasureRequest treasureRequest)
    {
        try
        {
            var result = await _awingService.CalculateFuel(treasureRequest);
            return Ok(result); 
        }
        catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}