
using Domain.Models;

namespace Domain.Interfaces;

public interface IItemsRepository
{
    Task<Item?> GetItem(int itemId);
}
