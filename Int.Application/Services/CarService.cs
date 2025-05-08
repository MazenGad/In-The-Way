using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Int.Domain.Services.Contrct;
using Int.Domain;
using Int.Domain.Entities;
using Int.Domain.DTOs.Cars;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Int.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace InT.Application.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryServices _cloudinaryServices;
        private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FiElSekkaContext _context;
		public CarService(IUnitOfWork unitOfWork, ICloudinaryServices cloudinaryServices, IMapper mapper , IHttpContextAccessor httpContextAccessor , FiElSekkaContext context )
        {
            _unitOfWork = unitOfWork;
            _cloudinaryServices = cloudinaryServices;
            _mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_context = context;

		}

		public async Task<bool> DeleteCarAsync(int carId)
        {
            var CarRepo = _unitOfWork.Repository<Car>();
			var car = await _context.Cars.FindAsync(carId); // جلب السيارة من قاعدة البيانات باستخدام المعرف

			if (car == null)
            {
                throw new KeyNotFoundException("السيارة غير موجودة.");
            }

            car.IsDeleted = true; // تعيين حالة الحذف

			await _unitOfWork.CompleteAsync();

            return true; // حذف ناجح
        }

        public async Task<IEnumerable<CarDTO>> GetAllCarsAsync()
        {
            var CarRepo = _unitOfWork.Repository<Car>(); // جلب الريبو الخاص بالسيارات
            var cars = await CarRepo.GetAllAsync(); // جلب جميع السيارات من قاعدة البيانات
            return _mapper.Map<IEnumerable<CarDTO>>(cars); // تحويل البيانات إلى DTO
        }

        //public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        //{
        //    return _mapper.Map<IEnumerable<BrandDTO>>(await _unitOfWork.Repository<Brand>().GetAllAsync());
        //}



        //public async Task<IEnumerable<ColorDTO>> GetAllColorsAsync()
        //{
        //    // return _mapper.Map<IEnumerable<ColorDTO>>(await _unitOfWork.Repository<Color>().GetAllAsync()); 
        //    var color = await _unitOfWork.Repository<Color>().GetAllAsync();
        //    var mappedcolor = _mapper.Map<IEnumerable<ColorDTO>>(color);
        //    return mappedcolor;

        //}

        public async Task<CarDTO> GetCarByIdAsync(int id)
        {
            var car = await _unitOfWork.Repository<Car>().GetByIdAsync(id);
            var mappedCar = _mapper.Map<CarDTO>(car);
            return mappedCar;

        }

        public async Task<CarDTO> UpdateCarAsync(int carId, UpdateCarDTO carDto)
        {
            var CarRepo = _unitOfWork.Repository<Car>();
            var car = await _context.Cars.FindAsync(carId); // جلب السيارة من قاعدة البيانات باستخدام المعرف

			if (car == null)
            {
                throw new KeyNotFoundException("السيارة غير موجودة.");
            }

            // تحديث بيانات السيارة
            car.Description = carDto.Description ?? car.Description;
            car.Location = carDto.Location ?? car.Location;
            car.PlateNumber = carDto.PlateNumber ?? car.PlateNumber;
			if (carDto.BrandID.HasValue)
				car.BrandId = carDto.BrandID.Value;

			if (carDto.ModelID.HasValue)
				car.ModelId = carDto.ModelID.Value;

			if (carDto.ColorID.HasValue)
				car.ColorId = carDto.ColorID.Value;



			await _unitOfWork.CompleteAsync(); // حفظ التعديلات في قاعدة البيانات

			var updatedCar = await _context.Cars
					.Include(c => c.Brand)
					.Include(c => c.Color)
					.Include(c => c.Model)
					.Include(c => c.CarPhotos)
					.FirstOrDefaultAsync(c => c.Id == car.Id);

			return _mapper.Map<CarDTO>(updatedCar);
		}

        public async Task<CarDTO> UploadCarAsync(UploadCarDTO carDto)
        {
			var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException("User is not authenticated.");
			}
			var car = new Car()
            {
                Description = carDto.Description,
                Location = carDto.Location,
                PlateNumber = carDto.PlateNumber,
                ColorId = carDto.ColorId,
				BrandId = carDto.BrandId,
				ModelId = carDto.ModelId,
				UserId = userId

			};

            var CarRepo = _unitOfWork.Repository<Car>();
            //upload data to cloudinary
            if (carDto.CarPhotos != null && carDto.CarPhotos.Count > 0)
            {

                var uploadResults = await _cloudinaryServices.UploadImagesAsync(carDto.CarPhotos);
                var carImages = uploadResults.Select(result => new CarPhoto
                {
                    carId = car.Id,
                    imageUrl = result.imageUrl,
                    publicId = result.publicId,
                }).ToList();

                foreach (var carImage in carImages)
                {
                    car.CarPhotos.Add(carImage);
                }
            }
            await CarRepo.AddAsync(car);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
				var savedCar = await _context.Cars
							.Include(c => c.Brand)
							.Include(c => c.Model)
							.Include(c => c.Color)
							.Include(c => c.CarPhotos)
							.FirstOrDefaultAsync(c => c.Id == car.Id);

				return _mapper.Map<CarDTO>(savedCar);
			}
            else
            {
                throw new Exception();
            }
        }


    }
}
