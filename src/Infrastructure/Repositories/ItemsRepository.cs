using Dapper;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly IDbConnection _connection;
        public ItemsRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Item?> GetItem(int itemId)
        {
            string sql = @"SELECT id, name FROM items
                            WHERE id = @Id;";
            return await _connection.QueryFirstOrDefaultAsync<Item>(sql, new { Id = itemId });
        }
    }
}
