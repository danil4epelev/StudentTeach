using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core
{
	public class FileData
	{
		public byte[] Content { get; set; }

		public MemoryStream ContentAsStream { get; set; }

		public string Name { get; set; }

		public string Extension { get; set; }
	}
}
