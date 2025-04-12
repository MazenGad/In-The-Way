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

namespace InT.Application.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryServices _cloudinaryServices;
        private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CarService(IUnitOfWork unitOfWork, ICloudinaryServices cloudinaryServices, IMapper mapper , IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryServices = cloudinaryServices;
            _mapper = mapper;
			_httpContextAccessor = httpContextAccessor;

		}

		public async Task<bool> DeleteCarAsync(int carId)
        {
            var CarRepo = _unitOfWork.Repository<Car>();
            var car = await CarRepo.GetByIdAsync(carId);

            if (car == null)
            {
                throw new KeyNotFoundException("السيارة غير موجودة.");
            }

            CarRepo.Delete(car);
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
            var car = await CarRepo.GetByIdAsync(carId);

            if (car == null)
            {
                throw new KeyNotFoundException("السيارة غير موجودة.");
            }

            // تحديث بيانات السيارة
            car.Description = carDto.Description ?? car.Description;
            car.Location = carDto.Location ?? car.Location;
            car.PlateNumber = carDto.PlateNumber ?? car.PlateNumber;
            car.Color = carDto.Color ?? car.Color;
            car.Brand = carDto.Brand ?? car.Brand;

            await _unitOfWork.CompleteAsync(); // حفظ التعديلات في قاعدة البيانات

            return _mapper.Map<CarDTO>(car);
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
                Color = carDto.Color,
                Brand = carDto.Brand,
                Model=carDto.Model,
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
                return _mapper.Map<CarDTO>(car);
            }
            else
            {
                throw new Exception();
            }
        }


    }
}
