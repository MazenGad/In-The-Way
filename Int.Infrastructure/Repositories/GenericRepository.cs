using Int.Domain.Entities;
using Int.Domain.Repositories.Contract;
using Int.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InT.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly FiElSekkaContext _context;

        public GenericRepository(FiElSekkaContext context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<TEntity>> GetAllAsync()
        //{
        //    if (typeof(TEntity) == typeof(Car))
        //    {
        //        return (IEnumerable<TEntity>)await _context.Cars.Include(P => P.BCodeNavigation)
        //             .Include(P => P.CCodeNavigation).Include(P => P.CarPhotos)
        //             .ToListAsync();
        //    }
        //    return await _context.Set<TEntity>().ToListAsync();
        //}

        //public async Task<TEntity?> GetByIdAsync(int id)
        //{
        //    if (typeof(TEntity) == typeof(Car))
        //    {
        //        var car = await _context.Cars.Include(P => P.BCodeNavigation)
        //             .Include(P => P.CCodeNavigation)
        //             .Include(P => P.CarPhotos)
        //             .FirstOrDefaultAsync(P => P.CId == id);
        //        return car as TEntity;
        //    }
        //    return await _context.Set<TEntity>().FindAsync(id);
        //}

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if (typeof(TEntity) == typeof(Car))
            {
                return (IEnumerable<TEntity>) await _context.Cars.Include(P => P.CarPhotos).OrderByDescending(c => c.CreateAt).ToListAsync();
                     
              
            }

            return await _context.Set<TEntity>().ToListAsync(); ; // جلب جميع البيانات من الجدول
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
			if (typeof(TEntity) == typeof(Car))
			{
				return await _context.Cars
					.Include(c => c.CarPhotos)
					.FirstOrDefaultAsync(c => c.Id == id) as TEntity;
			}
			else
			{
				return await _context.Set<TEntity>().FindAsync(id);
			}
		}
    }
}
