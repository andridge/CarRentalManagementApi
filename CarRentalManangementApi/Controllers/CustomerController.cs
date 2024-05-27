using CarRentalManagement.Module.Database;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalManangementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        public CustomerController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet] 
        public IActionResult Get()
        {
            try
            {
                XPCollection<Customer> customers = new XPCollection<Customer>(_unitOfWork);
                if (customers != null)
                {
                    return Ok(customers.Select(c => new
                    {
                        customerName = (c.custFname +" " +c.custLname)
                    })); 
                }return NotFound(); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{customerId}")]
        public IActionResult Get(Int64 customerId)
        {
            try
            {
                Customer customer = _unitOfWork.GetObjectByKey<Customer>(customerId);
                if(customer != null)
                 {  
                 return Ok(new {
                    customerPhone = customer.custPhone,
                 });
                 }
                return NotFound();
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut("{customerId}")]

        public IActionResult Put(Int64 customerId , [FromBody] string customerEmail)
        {
            try
            {
                Customer customer = _unitOfWork.GetObjectByKey<Customer>(customerId);
                if(customer != null)
                {
                    customer.custEmail = customerEmail;
                    _unitOfWork.Save(customer);
                    _unitOfWork.CommitChanges();
                    return Ok(customer);
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
