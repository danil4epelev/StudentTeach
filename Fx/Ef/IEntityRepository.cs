using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Ef
{
	public interface IEntityRepository<T> where T : class
	{
		IQueryable<T> GetAll();
		void Add(T entity);
		Task AddAsync(T entity);
		Task AddRangeAsync(T[] entities);
		void Update(T entity);
		Task UpdateAsync(T entity);
		void UpdateRange(T[] entities);
		void Delete(T entity);
		Task DeleteAsync(T entity);
		Task DeleteRangeAsync(T[] entities);
	}
}
