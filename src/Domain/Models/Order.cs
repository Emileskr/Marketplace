
namespace Domain.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DeleteAt { get; set; }
}
