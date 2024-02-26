using Domain.Models.DTOs;

namespace Domain.Interfaces;

public interface IUsersRepository
{
    Task InsertUser(UserDto user);
    Task<UserDto?> GetUser(int id);

}
