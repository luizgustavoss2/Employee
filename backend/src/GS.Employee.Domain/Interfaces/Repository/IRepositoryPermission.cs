using System;
using System.Threading.Tasks;

namespace GS.Employee.Domain.Interfaces.Repository
{
    public interface IRepositoryPermission
    {
        Task<int> CreateAsync(Domain.Entities.Permission permission);

        Task<int> UpdateAsync(Domain.Entities.Permission permission);
    }
}
