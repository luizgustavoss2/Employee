using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GS.Employee.Domain.Interfaces.Repository;
using GS.Employee.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GS.Employee.Infra.Data.Repositories
{
    public class RepositoryUserPermission : IRepositoryUserPermission
    {
        private readonly IConfiguration _configuration;

	    private SqlConnection OpenConnection() => new SqlConnection(_configuration.GetConnectionString("Sql"));

        private static readonly string INSERT = @"
	        Insert into [dbo].[UserPermission](
	            UserId,
	            PermissionId,
	            CreatedOn 
            ) 
            values(
	            @UserId,
	            @PermissionId,
	            @CreatedOn 
            );";

        private static readonly string UPDATE = @"
	        Update [dbo].[UserPermission] set
	           UserId = @UserId,
	           PermissionId = @PermissionId,
	           UpdatedOn = @UpdatedOn 
            Where Id = @Id;";

        private static readonly string DELETE = @"
	        Delete from [dbo].[UserPermission] 
            Where UserId = @UserId;";

        private static readonly string GET_BY_USERID = @"
            Select 
                Id,
                UserId,
	            PermissionId,
	            CreatedOn 
            from [dbo].[UserPermission]
            Where UserId = @UserId;";


        public RepositoryUserPermission(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> CreateAsync(UserPermission userPermission)
        {
            using var conn = OpenConnection();

            await conn.ExecuteAsync(INSERT, new
            {
	            userPermission.Id,
	            userPermission.UserId,
	            userPermission.PermissionId,
	            userPermission.CreatedOn 
            });

            return userPermission.Id;
        }

        public async Task<int> UpdateAsync(UserPermission userPhone)
        {
            using var conn = OpenConnection();

            userPhone.UpdatedOn = DateTime.Now;
            await conn.ExecuteAsync(UPDATE, new
            {
                userPhone.Id,
                userPhone.UserId,
                userPhone.PermissionId,
                userPhone.UpdatedOn
            });

            return userPhone.Id;
        }

        public async Task DeleteByUserIdAsync(Guid userId)
        {
            using var conn = OpenConnection();

            await conn.ExecuteAsync(DELETE, new
            {
                userId
            });
        }

        public async Task<IEnumerable<UserPermission>> GetByUserId(Guid? userId)
        {
            using var conn = OpenConnection();

            var user = await conn.QueryAsync<UserPermission>(GET_BY_USERID, new { UserId = userId });

            return user.ToList();
        }
    }
}
