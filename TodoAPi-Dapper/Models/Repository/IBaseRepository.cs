using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoAPi_Dapper.Models.Repository
{
    public interface IBaseRepository<TEntity>
    {
        Task<TEntity> FindAsync(string id);
        Task<IEnumerable<TEntity>> FindAllAsync();
        Task Add(TEntity entity);
        Task Remove(TEntity entity);
        Task Update(TEntity entity);
        int Count();
    }
}
