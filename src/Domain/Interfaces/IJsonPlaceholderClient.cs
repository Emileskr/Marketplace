using Domain.Models.DTOs;


namespace Domain.Interfaces;

public interface IJsonPlaceholderClient
{
    Task<JsonPlaceholderResult<UserDto>> GetUserAsync(int userId);

}
