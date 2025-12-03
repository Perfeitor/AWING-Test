namespace API.Models;

public class DpPoint
{
    public PointPos Pos { get; }
    public double Cost { get; }

    public DpPoint(PointPos pos, double cost)
    {
        Pos = pos;
        Cost = cost;
    }
}