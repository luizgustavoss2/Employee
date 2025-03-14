using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GS.Employee.Domain.Entities;
using GS.Employee.Domain.Interfaces.Repository;

namespace GS.Employee.Infra.Data.Repositories;

public class RepositoryUser(
    IConfiguration configuration) : IRepositoryUser
{
    private readonly IConfiguration _configuration = configuration;

    private SqlConnection OpenConnection() => new SqlConnection(_configuration.GetConnectionString("Sql"));

    private static readonly string INSERT = @"
        INSERT INTO [dbo].[User](
            Id,
            FirstName,
            LastName,
            Email,
            Document,
            BirthDate,
            ManagerId,
            Password,
            CreatedOn 
        ) 
        VALUES(
            @Id,
            @FirstName,
            @LastName,
            @Email,
            @Document,
            @BirthDate,
            @ManagerId,
            @Password,
            @CreatedOn 
        );";

    private static readonly string UPDATE = @"UPDATE [dbo].[User] SET UpdatedOn = @UpdatedOn ";

    private static readonly string GET_BY_NAME = @"
        SELECT 
            Id,
            FirstName,
            LastName,
            Email,
            Document,
            BirthDate,
            ManagerId,
            Password,
            CreatedOn, 
            UpdatedOn,
            DeletedOn     
        FROM [dbo].[User]
        WHERE (FirstName LIKE '%' + @Name + '%' OR LastName LIKE '%' + @Name + '%')
        AND DeletedOn IS NULL";

    private static readonly string GET_BY_FILTERS = @"
        SELECT 
            U.Id,
            U.FirstName,
            U.LastName,
            U.Email,
            U.Document,
            U.BirthDate,
            U.ManagerId,
            U.Password,
            U.CreatedOn,
            U.UpdatedOn,
            U.DeletedOn,
            M.Id,
            M.FirstName,
            M.LastName                  
        FROM [dbo].[User] U
        LEFT JOIN [dbo].[User] M ON U.ManagerId = M.Id
        WHERE (U.FirstName = @FirstName OR @FirstName IS NULL)
        AND (U.LastName = @LastName OR @LastName IS NULL)
        AND (U.Email = @Email OR @Email IS NULL)    
        AND (U.Document = @Document OR @Document IS NULL)  
        AND (U.ManagerId = @ManagerId OR @ManagerId IS NULL)  
        AND U.DeletedOn IS NULL 
        ORDER BY U.FirstName";

    private static readonly string GET_BY_USERID = @"
        SELECT 
            U.Id,
            U.FirstName,
            U.LastName,
            U.Email,
            U.Document,
            U.BirthDate,
            U.ManagerId,
            U.Password,
            U.CreatedOn,
            U.UpdatedOn,
            U.DeletedOn,
            M.Id,
            M.FirstName,
            M.LastName,
            M.ManagerId 
        FROM [dbo].[User] U
        LEFT JOIN [dbo].[User] M ON U.ManagerId = M.Id
        WHERE U.Id = @Id
        AND U.DeletedOn IS NULL";

    private static readonly string GET_BY_EMAIL = @"
        SELECT 
            U.Id,
            U.FirstName,
            U.LastName,
            U.Email,
            U.Document,
            U.BirthDate,
            U.ManagerId,
            U.Password                
        FROM [dbo].[User] U
        WHERE U.Email = @Email
        AND U.DeletedOn IS NULL";

    public async Task<Guid> CreateAsync(User user)
    {
        using var conn = OpenConnection();

        await conn.ExecuteAsync(INSERT, new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Document,
            user.BirthDate,
            user.ManagerId,
            user.Password,
            user.CreatedOn
        });
           
        return user.Id;
    }

    public async Task<Guid> UpdateAsync(User user)
    {
        using var conn = OpenConnection();

        var query = UPDATE;

        if (!string.IsNullOrEmpty(user.FirstName))
            query += ", FirstName = @FirstName";

        if (!string.IsNullOrEmpty(user.LastName))
            query += ", LastName = @LastName";

        if (!string.IsNullOrEmpty(user.Email))
            query += ", Email = @Email";

        if (!string.IsNullOrEmpty(user.Document))
            query += ", Document = @Document";

        if (!string.IsNullOrEmpty(user.BirthDate))
            query += ", BirthDate = @BirthDate";

        if (!string.IsNullOrEmpty(user.Password))
            query += ", Password = @Password";

        if (user.ManagerId != Guid.Empty)
            query += ", ManagerId = @ManagerId";

        query += " WHERE Id = @Id;";

        user.UpdatedOn = DateTime.Now;

        await conn.ExecuteAsync(query, new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Document,
            user.BirthDate,
            user.Password,
            user.ManagerId,
            user.UpdatedOn
        });

        return user.Id;
    }

    public async Task<User> GetByName(string name)
    {
        using var conn = OpenConnection();

        var user = await conn.QueryFirstOrDefaultAsync<User>(GET_BY_NAME, new
        {
            Name = name
        });

        return user;
    }

    public async Task<IEnumerable<User>> GetByFilters(string firstName, string lastName, string email, string document, Guid? managerId)
    {
        using var conn = OpenConnection();
      
        var users = await conn.QueryAsync<User, User, User>(GET_BY_FILTERS,
            map: (user, manager) =>
            {
                user.Manager = manager;
                return user;
            },
            param: new
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Document = document,
                ManagerId = managerId != Guid.Empty ? managerId : null
            });

        return users;
    }

    public async Task<User> GetByUserId(Guid? id)
    {
        using var conn = OpenConnection();

        var user = await conn.QueryAsync<User, User, User>(GET_BY_USERID,
            map: (user, manager) =>
            {
                user.Manager = manager;
                return user;
            },
            param: new
            {
                Id = id
            });

        var result = user.FirstOrDefault();

        return result;
    }

    public async Task<User> GetByEmail(string email)
    {
        using var conn = OpenConnection();

        var user = await conn.QueryAsync<User>(GET_BY_EMAIL, new { Email = email });

        var result = user.FirstOrDefault();

        return result;
    }
}