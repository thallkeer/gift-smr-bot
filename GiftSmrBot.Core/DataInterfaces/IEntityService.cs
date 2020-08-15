using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiftSmrBot.Core.DataInterfaces
{
    public interface IEntityService<T, TKey> where T : class, new()
    {
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(TKey id);
        Task CreateRangeAsync(IEnumerable<T> entities);
        Task CreateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteAsync(TKey id);
    }
}
