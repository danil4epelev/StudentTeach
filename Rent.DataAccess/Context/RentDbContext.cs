using Microsoft.EntityFrameworkCore;
using Rent.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.DataAccess.Context
{
	public class RentDbContext : DbContext
	{
		public RentDbContext([NotNull] DbContextOptions options) : base (options)
		{

		}

		public virtual DbSet<RentItemPropertiesConnection> RentItemPropertiesConnection { get; set; }
		public virtual DbSet<ChapterPropertiesConnection> ChapterPropertiesConnection { get; set; }
		public virtual DbSet<Properties> Properties { get; set; }
		public virtual DbSet<User> User { get; set; }
		public virtual DbSet<Chapter> Chapter { get; set; }
		public virtual DbSet<RentItem> RentItem { get; set; }
	}
}
