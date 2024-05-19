using Microsoft.Extensions.Caching.Memory;
using Rent.Core.Contracts.Helpers;
using Rent.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Helpers
{
    public class CacheHelper : ICacheHelper
	{
		private readonly IMemoryCache _memoryCache;
		private readonly MemoryCacheEntryOptions _cacheEntryOptions;

		public CacheHelper(
			IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
			_cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(12));
		}

		public Guid SetApproveModel(RegisterUserModelsExpanded model)
		{
			var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
			var modelId = Guid.NewGuid();
			_memoryCache.Set(modelId, model, cacheEntryOptions);
			return modelId;
		}

		public RegisterUserModelsExpanded GetApproveModel(Guid modelId)
		{
			if (_memoryCache.TryGetValue<RegisterUserModelsExpanded>(modelId, out var model))
			{
				return model;
			}

			return null;
		}
	}
}
