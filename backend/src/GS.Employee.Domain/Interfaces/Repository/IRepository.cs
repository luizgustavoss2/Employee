using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GS.Employee.Domain.Interfaces.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteAsync(int id);
        Task<bool> UpdateAsync<TEntity>(TEntity entity) where TEntity : class;
        Task<TEntity> GetAsync<TEntity>(Guid id);
        Task<TEntity> GetAsync<TEntity>(int id);
        Task<IEnumerable<TEntity>> GetAsync<TEntity>();
        Task<int> InsertAsync<TEntity>(TEntity entity) where TEntity : class;
    }
}