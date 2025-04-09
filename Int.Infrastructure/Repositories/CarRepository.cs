using Int.Domain.Entities;
using Int.Domain.Repositories.Contract;
using Int.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InT.Infrastructure.Repositories
{
    public class CarRepository : GenericRepository<Car>, ICarRepository
    {
        private readonly FiElSekkaContext _context;

		public CarRepository(FiElSekkaContext context) : base(context) 
		{
			_context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
