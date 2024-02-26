
namespace Domain.Models;

public class UpdateOrderStatusModel
{
    public int OrderId { get; set; }
    public string Status { get; set; } = "created";
}
