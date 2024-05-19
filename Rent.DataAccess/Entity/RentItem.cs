using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.DataAccess.Entity
{
	public class RentItem : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public long ChapterId { get; set; }
		public decimal Price { get; set; }
		public int PriceType { get; set; }
		public long AuthorId { get; set; }
		public Chapter Chapter { get; set; }
		public User Author { get; set; }
		public User Moderator { get; set; }
		public bool IsModerated { get; set; }
		public DateTime DtCreate { get; set; }
		public DateTime? DtApprove { get; set; }
		public DateTime? DtUpToSearch { get; set; }
		public int Status { get; set; }
		public DateTime? DtSendToModeration { get; set; }
		public string RejectedRemarks { get; set; }
	}
}
