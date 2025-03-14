using GS.Employee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GS.Employee.Domain.Interfaces.Repository
{
    public interface IRepositoryUserPhone
    {
        Task<int> CreateAsync(UserPhone userPhone);

        Task<int> UpdateAsync(UserPhone userPhone);

        Task DeleteByUserIdAsync(Guid userId);

        Task<IEnumerable<UserPhone>> GetByUserId(Guid? userId);
    }
}
