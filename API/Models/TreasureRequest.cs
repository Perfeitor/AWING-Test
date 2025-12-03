using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace API.Models;

[Table("TreasureRequest")]
public class TreasureRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Range(1,500)]
    public int N { get; set; }

    [Range(1,500)]
    public int M { get; set; }

    [Range(1, 250000)]
    public int P { get; set; }
    
    [NotMapped]
    public int[][] Matrix { get; set; } = null!;
    
    public string? MatrixJson { get; set; }
    
    public ICollection<TreasureResult> Results { get; set; } = new List<TreasureResult>();
}