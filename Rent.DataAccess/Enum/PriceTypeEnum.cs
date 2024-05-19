using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.DataAccess.Enum
{
	public enum PriceTypeEnum
	{
		Day = 10,
		Hour = 20
	}

	public class PriceTypeEnumChunk
	{
		public string Name { get; set; }
		public int Value { get; set; }

		public static List<PriceTypeEnumChunk> GetList()
		{
			return new List<PriceTypeEnumChunk>
			{
				new PriceTypeEnumChunk { Name = "Day", Value = 10 },
				new PriceTypeEnumChunk { Name = "Hour", Value = 20 }
			};
		}
	}
}
