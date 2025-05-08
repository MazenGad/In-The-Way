using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int.Domain.Entities
{
    public class Color
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<Car> Cars { get; set; } = new List<Car>();
	}
}
