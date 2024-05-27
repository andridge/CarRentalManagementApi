using CarRentalManagement.Module.Database;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalManangementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {

        private readonly UnitOfWork _unitOfWork;

        public CarController(UnitOfWork unitOfWork)
        {
              _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                XPCollection<Car> car = new XPCollection<Car>(_unitOfWork);
                if (car != null)
                {
                    return Ok(car);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

       [ HttpGet ("{carId}") ]
       public IActionResult  Get(Int64 carId)
        {
            try
            {
                Car car = _unitOfWork.GetObjectByKey<Car>(carId);
                if(car != null)
                {
                    return Ok(new
                    {
                        carId = car.carId,
                        carName = car.carName,
                        carHourlyRate = car.carHourlyRate,
                        carStatus = car.carStatus,
                    });
                } return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut ("{carId}")]
        public IActionResult Put(Int64 carId , [FromBody] decimal carHourlyRate)
        {

            try { 
            Car car  = _unitOfWork.GetObjectByKey<Car>(carId);
            if (car != null)
              {
                car.carHourlyRate = carHourlyRate;
               // car.carStatus = CarStatus.notActive;
                _unitOfWork.Save(car);
                _unitOfWork.CommitChanges();
                return Ok(car);
              }return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
