using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPrism200.Infrastructure.Models
{
	public class Stock
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Symbol { get; set; }

        public string Name { get; set; }
    }
}
