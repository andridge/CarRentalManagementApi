using DevExpress.Xpo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarRentalManagement.Module.BusinessObjects;
using CarRentalManagement.Module.Database;
using System.Security.Cryptography;
using DevExpress.Persistent.BaseImpl;


namespace CarRentalManangementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public EventController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                XPCollection<Event> events = new XPCollection<Event>(_unitOfWork);
                if (events != null)
                {
                    return Ok(events.Select(E => new
                    {
                        eventName = E.Subject,
                        eventStartDate = E.StartOn,
                        eventEndDate = E.EndOn,

                    }));
                }
                return NotFound();


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        //[HttpGet("{Oid}")]
        //public IActionResult Get(Char Oid)
        //{
        //    try
        //    {
        //       Event event = _unitOfWork.GetObjectByKey<Event>(Oid);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}
    }
}
