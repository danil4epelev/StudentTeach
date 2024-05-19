﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.DataAccess.Entity
{
	public class BaseEntity
	{
		[Key]
		public long Id { get; set; }
		public Guid Uid { get; set; }
	}
}
