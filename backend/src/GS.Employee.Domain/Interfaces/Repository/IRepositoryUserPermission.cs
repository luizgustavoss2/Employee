using GS.Employee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GS.Employee.Domain.Interfaces.Repository
{
    public interface IRepositoryUserPermission
    {
        Task<int> CreateAsync(UserPermission userPermission);

        Task<int> UpdateAsync(UserPermission userPhone);

        Task DeleteByUserIdAsync(Guid userId);

        Task<IEnumerable<UserPermission>> GetByUserId(Guid? userId);
    }
}
