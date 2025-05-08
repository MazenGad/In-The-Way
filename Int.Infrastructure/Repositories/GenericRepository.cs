using Int.Domain.DTOs.Cars;
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

		public async Task<IEnumerable<CarDTO>> GetAllAsync()
		{
			var cars = await _context.Cars
				.Where(c => !c.IsDeleted)
				.Include(c => c.CarPhotos)
				.Include(c => c.Brand)
				.Include(c => c.Model)
				.Include(c => c.Color)
				.OrderByDescending(c => c.CreateAt)
				.Select(c => new CarDTO
				{
					Id = c.Id,
					Description = c.Description,
					PlateNumber = c.PlateNumber,
					Location = c.Location,
					BrandName = c.Brand.Name,
					ModelName = c.Model.Name,
					ColorName = c.Color.Name,
					BrandId = c.BrandId,
					ModelId = c.ModelId,
					ColorId = c.ColorId,
					imageUrls = c.CarPhotos.Select(p => p.imageUrl).ToList(),
					CreatedAt = c.CreateAt,
					UserId = c.UserId
				})
				.ToListAsync();

			return cars;
		}



		public async Task<CarDTO?> GetByIdAsync(int id)
		{
			var car = await _context.Cars
				.Include(c => c.Brand)
				.Include(c => c.Model)
				.Include(c => c.Color)
				.Include(c => c.CarPhotos)
				.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

			if (car == null)
				throw new KeyNotFoundException("Car not found");

			return new CarDTO
			{
				Id = car.Id,
				Description = car.Description,
				Location = car.Location,
				PlateNumber = car.PlateNumber,
				BrandName = car.Brand.Name,
				ModelName = car.Model.Name,
				ColorName = car.Color.Name,
				BrandId = car.BrandId,
				ModelId = car.ModelId,
				ColorId = car.ColorId,
				imageUrls = car.CarPhotos.Select(p => p.imageUrl).ToList(),
				CreatedAt = car.CreateAt,
				UserId = car.UserId

			};
		}



	}
}
