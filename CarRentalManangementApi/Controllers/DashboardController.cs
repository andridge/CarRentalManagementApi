using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalManangementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public DashboardController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                XPCollection<DashboardData> dashboardDatas = new XPCollection<DashboardData>(_unitOfWork);
                if (dashboardDatas != null) 
                {
                    return Ok(dashboardDatas);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
