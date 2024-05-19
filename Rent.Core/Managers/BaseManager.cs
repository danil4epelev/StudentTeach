using AutoMapper;
using Fx.Ef;
using Rent.DataAccess.Entity;

namespace Rent.Core.Managers
{
	public abstract class BaseManager<TEntity, TData> where TEntity : BaseEntity
	{
		private readonly IEntityRepository<TEntity> _repository;
		private readonly IMapper _mapper;

		protected BaseManager(IEntityRepository<TEntity> repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public virtual async Task<long> AddAsync(TData data)
		{
			var entity = _mapper.Map<TEntity>(data);
			entity.Uid = Guid.NewGuid();
			await _repository.AddAsync(entity);
			return entity.Id;
		}

		public virtual async Task AddRangeAsync(IEnumerable<TData> dataSet)
		{
			var entities = new List<TEntity>();
			foreach (var data in dataSet)
			{
				entities.Add(_mapper.Map<TEntity>(data));
			}
			await _repository.AddRangeAsync(entities.ToArray());
		}

		public virtual async Task DeleteAsync(long entityId)
		{
			var entity = _repository.GetAll().Where(t => t.Id == entityId).SingleOrDefault();
			if (entity != null)
			{
				await _repository.DeleteAsync(entity);
			}
		}

		public virtual async Task UpdateAsync(long entityId, TData data)
		{
			var entity = _repository.GetAll().Where(t => t.Id == entityId).SingleOrDefault();
			if (entity != null)
			{
				_mapper.Map(data, entity);
				await _repository.UpdateAsync(entity);
			}
		}

		public virtual IQueryable<TData> GetList()
		{
			var allEntities = _repository.GetAll();
			return _mapper.ProjectTo<TData>(allEntities);
		}
	}
}
