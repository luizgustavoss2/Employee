using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GS.Employee.Domain.Interfaces.Repository;

namespace GS.Employee.Infra.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IConfiguration _configuration;
        public Repository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection OpenConnection() => new SqlConnection(_configuration.GetConnectionString("Sql"));
        public async Task<int> DeleteAsync(Guid id)
        {
            using var conn = OpenConnection();
            var query = $"UPDATE [{typeof(T).Name}] SET DeletedOn = GETDATE() WHERE Id = @id";
            return await conn.ExecuteAsync(query, new { id });
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = OpenConnection();
            var query = $"UPDATE [{typeof(T).Name}] SET DeletedOn = GETDATE() WHERE Id = @id";
            return await conn.ExecuteAsync(query, new { id });
        }

        public async Task<IEnumerable<TEntity>> GetAsync<TEntity>()
        {
            using var conn = OpenConnection();
            return await conn.QueryAsync<TEntity>($"SELECT * FROM [{typeof(T).Name}] WHERE DeletedOn IS NULL");
        }

        public async Task<TEntity> GetAsync<TEntity>(Guid id)
        {
            using var conn = OpenConnection();
            return await conn.QueryFirstOrDefaultAsync<TEntity>($"SELECT * FROM [{typeof(T).Name}] WHERE DeletedOn IS NULL AND Id = @id", new { id });
        }

        public async Task<TEntity> GetAsync<TEntity>(int id)
        {
            using var conn = OpenConnection();
            return await conn.QueryFirstOrDefaultAsync<TEntity>($"SELECT * FROM [{typeof(T).Name}] WHERE DeletedOn IS NULL AND Id = @id", new { id });
        }

        public async Task<int> InsertAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using var conn = OpenConnection();
            return await conn.InsertAsync(entity);
        }

        public async Task<bool> UpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using var conn = OpenConnection();
            return await conn.UpdateAsync(entity);
        }
    }
}
