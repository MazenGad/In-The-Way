using Int.Domain.DTOs.Cars;
using Int.Domain.Services.Contrct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Int.Api.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }
        //    [Authorize(Roles = "admin")]

        //    [HttpGet]
        //    public async Task<IActionResult> GatAllCars()



        //        var rsult = await _carService.GetAllCarsAsync();
        //        return Ok(rsult);
        //}

        //[HttpGet("Brands")]
        //public async Task<IActionResult> GatAllBrans()
        //{
        //    var result = await _carService.GetAllBrandsAsync();
        //    return Ok(result);
        //}
        //[HttpGet("Colors")]
        //public async Task<IActionResult> GatAllColors()
        //{
        //    var result = await _carService.GetAllCarsAsync();
        //    return Ok(result);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetCarById(int? id)
        //{
        //    if (id is null) return BadRequest("Invalid Id");

        //    var result = await _carService.GetCarByIdAsync(id.Value);

        //    if (result is null) return NotFound("car with this id is not found");
        //    return Ok(result);
        //}

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCar(UploadCarDTO carDto)
        {
            try
            {
                var car = await _carService.UploadCarAsync(carDto);
                return Ok(new { Message = "Car uploaded successfully", car });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error uploading car", Error = ex.Message });
            }
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CarDTO>>> GetAllCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            try
            {
                var car = await _carService.GetCarByIdAsync(id);
                return Ok(car);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] UpdateCarDTO carDto)
        {
            try
            {
                var updatedCar = await _carService.UpdateCarAsync(id, carDto);
                return Ok(updatedCar);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                var result = await _carService.DeleteCarAsync(id);
                return result ? Ok(new { message = "تم حذف السيارة بنجاح." }) : BadRequest();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }
}
