using System.Linq;
using System.Threading.Tasks;

namespace Rent.Core.Contracts.Managers
{
    public interface IBaseManager<TData>
    {
        Task<long> AddAsync(TData data);
        Task DeleteAsync(long dataId);
        IQueryable<TData> GetList();
        Task UpdateAsync(long dataId, TData data);
    }
}
