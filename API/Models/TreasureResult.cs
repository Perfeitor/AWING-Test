using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("TreasureResult")]
public class TreasureResult
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SessionId { get; set; }

    public double ResultFuel { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual TreasureRequest TreasureRequest { get; set; } = null!;
}