using AutoMapper;
using Fx.Ef;
using Rent.Core.Contracts.Managers;
using Rent.Core.Managers.Data;
using Rent.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Managers
{
	public class PropertiesManager : BaseManager<Properties, PropertiesData>, IPropertiesManager
	{
		public PropertiesManager(IEntityRepository<Properties> repository, IMapper mapper) : base(repository, mapper)
		{
		}
	}
}
