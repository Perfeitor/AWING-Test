using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class TreasureRequest
{
    [Range(1,500)]
    public int N { get; set; }

    [Range(1,500)]
    public int M { get; set; }

    [Range(1, 250000)]
    public int P { get; set; }

    public int[][] Matrix { get; set; } = null!;
}