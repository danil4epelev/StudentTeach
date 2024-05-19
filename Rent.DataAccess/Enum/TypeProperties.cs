using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.DataAccess.Enum
{
	public enum TypeProperties
	{
		String,
		Integer,
		Boolean,
		Decimal,
		Double,
		Enum
	}

	public class TypePropertiesChunk
	{
		public string Name { get; set; }
		public int Value { get; set; }

		public static List<TypePropertiesChunk> GetList()
		{
			return new List<TypePropertiesChunk>
			{
				new TypePropertiesChunk { Name = "String", Value = 0 },
				new TypePropertiesChunk { Name = "Integer", Value = 1 },
				new TypePropertiesChunk { Name = "Boolean", Value = 2 },
				new TypePropertiesChunk { Name = "Decimal", Value = 3},
				new TypePropertiesChunk { Name = "Double", Value = 4 },
				new TypePropertiesChunk { Name = "Enum", Value = 5 },
			};
		}
	}
}
