using CarRentalManagement.Module.Database;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalManangementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
       // private UnitOfWork _unitOfWork;

        public BookingController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {  
                XPCollection<Booking> bookings = new XPCollection<Booking>(_unitOfWork);
                if (bookings != null)
                {
                    return Ok(bookings);
                }
                return NotFound();
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet ("{bookingId}")]
        public IActionResult Get(Int64 bookingId)
        {
            try
            {
                Booking booking = _unitOfWork.GetObjectByKey<Booking>(bookingId);
                if(booking != null)
                {
                    return Ok(new
                    {
                        bookingId = booking.bookingId,
                        bookingStartDate = booking.bookingStartDate,
                        bookingEndDate = booking.bookingEndDate,
                        bookingDays = booking.bookingDays,
                        bookingStatus = booking.bookingStatus
                    });
                }return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        //update end date status and duration in int days 
        [HttpPut("{bookingId}")]
        public IActionResult Put(Int64 bookingId , [FromBody] DateTime bookingEndDate)
        {
            try
            {
                Booking booking = _unitOfWork.GetObjectByKey<Booking>(bookingId);
                if (booking != null)
                {
                  
                    booking.bookingEndDate = bookingEndDate;
                    booking.bookingDays = (int)(booking.bookingEndDate - booking.bookingStartDate).Duration().TotalDays;
                    booking.bookingStatus = BookingStatus.done;
                    _unitOfWork.Save(booking);
                    _unitOfWork.CommitChanges();
                    return Ok(booking);
                }return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
