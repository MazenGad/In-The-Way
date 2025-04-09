using Int.Domain.DTOs.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int.Domain.Services.Contrct
{
	public interface ICarService
    {
        Task<IEnumerable<CarDTO>> GetAllCarsAsync();
        //Task<IEnumerable<BrandDTO>> GetAllBrandsAsync();
        //   Task<IEnumerable<ColorDTO>> GetAllColorsAsync();
          Task<CarDTO> GetCarByIdAsync(int id);

        Task<CarDTO> UploadCarAsync(UploadCarDTO carDto);

        //Task<CarDTO> AddCarAsync(CarUploadDTO carDto);
        
            Task<CarDTO> UpdateCarAsync(int carId, UpdateCarDTO carDto);
            Task<bool> DeleteCarAsync(int carId);
        


    }
}
