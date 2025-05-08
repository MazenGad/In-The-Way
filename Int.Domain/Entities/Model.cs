using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int.Domain.Entities
{
	public class Model
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public int BrandId { get; set; }      
		public Brand Brand { get; set; }     

		public ICollection<Car> Cars { get; set; } = new List<Car>();
	}

}
