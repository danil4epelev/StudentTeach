using AutoMapper;
using Fx.Ef;
using Rent.Core.Contracts.Managers;
using Rent.Core.Managers.Data;
using Rent.DataAccess.Entity;
using Rent.DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Managers
{
	public class RentItemManager : BaseManager<RentItem, RentItemData>, IRentItemManager
	{
		public RentItemManager(IEntityRepository<RentItem> repository, IMapper mapper) : base(repository, mapper)
		{
		}

		public IQueryable<RentItemData> GetActiveItems()
		{
			return GetList().Where(t => t.Status == (int)RentItemStatusEnum.Active);
		}
	}
}
