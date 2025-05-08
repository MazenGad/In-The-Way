using Int.Domain.DTOs.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int.Domain.Repositories.Contract
{
	public interface IGenericRepository<TEntity> where TEntity : class
    {
          Task<IEnumerable<CarDTO>> GetAllAsync();
         Task<CarDTO> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);

        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
