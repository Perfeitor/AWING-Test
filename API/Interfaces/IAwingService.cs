using API.Models;

namespace API.Interfaces;

public interface IAwingService
{
    public double CalculateFuel(TreasureRequest treasureRequest);
}