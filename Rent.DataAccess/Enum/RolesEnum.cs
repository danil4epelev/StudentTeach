using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Rent.DataAccess.Enum
{
	public enum RolesEnum
	{
		Default = 10,
		Admin = 20
	}

	public class RolesChunk
	{
		public string Name { get; set; }
		public int Value { get; set; }

		public static List<RolesChunk> GetList()
		{
			return new List<RolesChunk>
			{
				new RolesChunk { Name = "Default", Value = 10 },
				new RolesChunk { Name = "Admin", Value = 20 }
			};
		}
	}
}
