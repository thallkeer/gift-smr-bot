using System.Linq;
using System.Threading.Tasks;

namespace GiftSmrBot.Core.DataInterfaces
{
    public interface IEntityService<T, TKey> where T : class, new()
    {
        IQueryable<T> GetAll();
        Task<T> GetById(TKey id);
        Task Create(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Delete(TKey id);
    }
}
