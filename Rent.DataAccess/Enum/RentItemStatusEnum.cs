using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.DataAccess.Enum
{
	public enum RentItemStatusEnum
	{
		Draft = 10,
		Active = 20,
		NotActive = 30,
		Moderated = 40,
		Rejected = 50
	}

	public class RentItemStatusEnumChunk
	{
		public string Name { get; set; }
		public int Value { get; set; }

		public static List<RentItemStatusEnumChunk> GetList()
		{
			return new List<RentItemStatusEnumChunk>
			{
				new RentItemStatusEnumChunk { Name = "Draft", Value = 10 },
				new RentItemStatusEnumChunk { Name = "Active", Value = 20 },
				new RentItemStatusEnumChunk { Name = "NotActive", Value = 30 },
				new RentItemStatusEnumChunk { Name = "Moderated", Value = 40 },
			};
		}
	}
}
