using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GS.Employee.Domain.Interfaces.Repository
{
    public interface IRepositoryUser
    {
        Task<Guid> CreateAsync(Domain.Entities.User user);

        Task<Guid> UpdateAsync(Domain.Entities.User user);

        Task<Domain.Entities.User> GetByName(string name);

        Task<IEnumerable<Domain.Entities.User>> GetByFilters(string firstName, string lastName, string email, string document, Guid? managerId);

        Task<Domain.Entities.User> GetByUserId(Guid? id);
        Task<Domain.Entities.User> GetByEmail(string email);
    }
}
