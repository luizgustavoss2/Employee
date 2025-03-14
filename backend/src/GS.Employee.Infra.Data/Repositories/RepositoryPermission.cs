using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GS.Employee.Domain.Interfaces.Repository;

namespace GS.Employee.Infra.Data.Repositories
{
    public class RepositoryPermission : IRepositoryPermission
    {
        private readonly IConfiguration _configuration;

	    private SqlConnection OpenConnection() => new SqlConnection(_configuration.GetConnectionString("Sql"));

        private static readonly string INSERT = @"
	        Insert into [dbo].[Permission](
	            Description,
	            CreatedOn 
            ) 
            values(
	            @Description,
	            @CreatedOn 
            );";

        private static readonly string UPDATE = @"
	        Update [dbo].[Permission] set
	           Description = @Description,
	           UpdatedOn = @UpdatedOn 
            Where Id = @Id;";

        public RepositoryPermission(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> CreateAsync(Domain.Entities.Permission permission)
        {
            using var conn = OpenConnection();

            await conn.ExecuteAsync(INSERT, new
            {
	            permission.Id,
	            permission.Description,
	            permission.CreatedOn 
            });

            return permission.Id;
        }

        public async Task<int> UpdateAsync(Domain.Entities.Permission permission)
        {
            using var conn = OpenConnection();

            permission.UpdatedOn = DateTime.Now; 
            await conn.ExecuteAsync(UPDATE, new
            {
	           permission.Id,
	           permission.Description,
	           permission.UpdatedOn 
            });

            return  permission.Id;
        }
    }
}
