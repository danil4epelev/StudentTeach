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
    public class ChapterManager : BaseManager<Chapter, ChapterData>, IChapterManager
	{
		public ChapterManager(IEntityRepository<Chapter> repository, IMapper mapper) : base(repository, mapper)
		{
		}
	}
}
