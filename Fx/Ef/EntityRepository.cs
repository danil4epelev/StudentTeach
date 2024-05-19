using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Fx.Ef
{
	public sealed class EntityRepository<T> : IEntityRepository<T> where T : class
	{
		public EntityRepository(DbContext dbContext)
		{
			_dbContext = dbContext;
		}

		#region Implementation of IEntityRepository<T>

		public void Add(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Added;
			_dbContext.SaveChanges();
		}

		public IQueryable<T> GetAll()
		{
			return _dbContext.Set<T>().AsNoTracking();
		}

		public void Update(T entity)
		{
			_dbContext.Set<T>().Update(entity);
			_dbContext.Entry(entity).State = EntityState.Modified;
			_dbContext.SaveChanges();
		}

		public void UpdateRange(T[] entities)
		{
			_dbContext.Set<T>().UpdateRange(entities);
			foreach (var entity in entities)
			{
				_dbContext.Entry(entity).State = EntityState.Modified;
			}
			_dbContext.SaveChanges();
		}

		public void UpdateRangeAsync(T[] entities)
		{
			_dbContext.Set<T>().UpdateRange(entities);
			foreach (var entity in entities)
			{
				_dbContext.Entry(entity).State = EntityState.Modified;
			}
			_dbContext.SaveChanges();
		}

		public void Delete(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Deleted;
			_dbContext.Set<T>().Remove(entity);
			_dbContext.SaveChanges();
		}


		public async Task AddAsync(T entity)
		{
			await _dbContext.Set<T>().AddAsync(entity);
			await Save();
			_dbContext.Entry(entity).State = EntityState.Detached;
			await Save();
		}

		public async Task AddRangeAsync(T[] entities)
		{
			await _dbContext.Set<T>().AddRangeAsync(entities);
			await Save();

			foreach (var entity in entities)
			{
				_dbContext.Entry(entity).State = EntityState.Detached;
			}
			await Save();
		}

		public async Task UpdateAsync(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
			await Save();

			_dbContext.Entry(entity).State = EntityState.Detached;
			await Save();
		}

		public async Task DeleteAsync(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Deleted;
			_dbContext.Set<T>().Remove(entity);
			await Save();
		}

		public async Task DeleteRangeAsync(T[] entities)
		{
			foreach (var entity in entities)
			{
				_dbContext.Entry(entity).State = EntityState.Deleted;
			}
			_dbContext.Set<T>().RemoveRange(entities);
			await Save();
		}

		private async Task Save()
		{
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);
		}

		public void Dispose()
		{
			_dbContext.Dispose();

			GC.SuppressFinalize(this);
		}

		#endregion

		private readonly DbContext _dbContext;
	}
}
