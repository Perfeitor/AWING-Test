using API.Models;

namespace API.Interfaces;

public interface IAwingService
{
    public Task<double> CalculateFuel(TreasureRequest treasureRequest);
}