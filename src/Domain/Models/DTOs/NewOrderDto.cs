
namespace Domain.Models.DTOs;

public class NewOrderDto
{
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public string Status { get; set; } = "created";
}
