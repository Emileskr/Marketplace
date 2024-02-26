using Dapper;
using Domain.Interfaces;
using Domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly IDbConnection _connection;
    public UsersRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<UserDto?> GetUser(int id)
    {
        string sql = @"SELECT name FROM users WHERE id = @Id;";
        var user = await _connection.QueryFirstOrDefaultAsync<UserDto>(sql, new { Id = id });
        return user;
    }

    public async Task InsertUser(UserDto user)
    {
        string sql = @"INSERT INTO users (id, name, email) VALUES 
                        (@Id, @Name, @Email) RETURNING Id";
        var args = new
        {
            Id = user.Id,
            Name = user.UserName,
            Email = user.Email
        };
        await _connection.ExecuteAsync(sql, args);
    }
}
