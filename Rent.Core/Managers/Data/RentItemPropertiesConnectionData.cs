using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Managers.Data
{
	public class RentItemPropertiesConnectionData
	{
		public long Id { get; set; }
		public Guid Uid { get; set; }
		public long RentItemId { get; set; }
		public long PropertiesId { get; set; }
		public int? IntValue { get; set; }
		public string? StringValue { get; set; }
		public bool? BooleanValue { get; set; }
		public decimal? DecimalValue { get; set; }
		public double? DoubleValue { get; set; }
	}
}
