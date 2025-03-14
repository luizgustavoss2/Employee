using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GS.Employee.Domain.Interfaces.Repository;
using GS.Employee.Domain.Entities;
using System.Linq;
using System.Collections.Generic;

namespace GS.Employee.Infra.Data.Repositories
{
    public class RepositoryUserPhone : IRepositoryUserPhone
    {
        private readonly IConfiguration _configuration;

	    private SqlConnection OpenConnection() => new SqlConnection(_configuration.GetConnectionString("Sql"));

        private static readonly string INSERT = @"
	        Insert into [dbo].[UserPhone](
	            UserId,
	            Phone,
	            CreatedOn 
            ) 
            values(
	            @UserId,
	            @Phone,
	            @CreatedOn 
            );";

        private static readonly string UPDATE = @"
	        Update [dbo].[UserPhone] set
	           UserId = @UserId,
	           Phone = @Phone,
	           UpdatedOn = @UpdatedOn 
            Where Id = @Id;";

        private static readonly string DELETE = @"
	        Delete from [dbo].[UserPhone] 
            Where UserId = @UserId;";

        private static readonly string GET_BY_USERID = @"
            Select 
                Id,
                UserId,
	            Phone,
	            CreatedOn 
            from [dbo].[UserPhone]
            Where UserId = @UserId;";

        public RepositoryUserPhone(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> CreateAsync(UserPhone userPhone)
        {
            using var conn = OpenConnection();

            await conn.ExecuteAsync(INSERT, new
            {
	            userPhone.UserId,
	            userPhone.Phone,
	            userPhone.CreatedOn 
            });

            return userPhone.Id;
        }

        public async Task<int> UpdateAsync(UserPhone userPhone)
        {
            using var conn = OpenConnection();

            userPhone.UpdatedOn = DateTime.Now; 
            await conn.ExecuteAsync(UPDATE, new
            {
	           userPhone.Id,
	           userPhone.UserId,
	           userPhone.Phone,
	           userPhone.UpdatedOn 
            });

            return  userPhone.Id;
        }


        public async Task DeleteByUserIdAsync(Guid userId)
        {
            using var conn = OpenConnection();

            await conn.ExecuteAsync(DELETE, new
            {
                userId
            });

        }

        public async Task<IEnumerable<UserPhone>> GetByUserId(Guid? userId)
        {
            using var conn = OpenConnection();

            var user = await conn.QueryAsync<UserPhone>(GET_BY_USERID, new { UserId = userId });

            return user.ToList();
        }
    }
}
