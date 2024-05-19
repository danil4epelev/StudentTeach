using Rent.Core.Managers;
using Rent.Core.Managers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Contracts.Managers
{
	public interface IRentItemManager : IBaseManager<RentItemData>
	{
		public IQueryable<RentItemData> GetActiveItems();
	}
}
