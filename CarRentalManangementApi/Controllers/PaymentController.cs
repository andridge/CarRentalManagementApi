
using CarRentalManagement.Module.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DevExpress.Xpo;
using Payment = CarRentalManagement.Module.Database.Payment;
using DevExpress.ExpressApp.Xpo;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Text;
using CarRentalManagement.Module.Controllers;
namespace CarRentalManangementApi.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase  {        

        private readonly UnitOfWork _unitOfWork;

        public PaymentController(UnitOfWork unitOfWork)
        {
             _unitOfWork = unitOfWork;
        }
        private readonly CarRentalManagement.Module.Controllers.PaymentController _paymentController;


        [HttpGet]
       // [ProducesResponseType(typeof(Payment[]), StatusCodes.Status200OK)]
      //  [ProducesResponseType(StatusCodes.Status400BadRequest)]

      //get all 
        public  IActionResult Get()
        {
            try
            {
                XPCollection<Payment> payments = new XPCollection<Payment>(_unitOfWork);
                if (payments != null)
                {
                    return Ok(payments.Select(p => new
                    {
                        paymentName = p.paymentName,
                        paymentAmount = p.paymentAmount
                    }));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //get with index
        [HttpGet("{paymentId}")]
        public IActionResult Get(Int64 paymentId)
        {
            try
            {
             
                    Payment payment = _unitOfWork.GetObjectByKey<Payment>(paymentId);
                    if (payment == null)
                    {
                        return NotFound();
                    }
                    Booking a = payment.bookingId;
                    Booking booking = _unitOfWork.GetObjectByKey<Booking>(a.bookingId);
                  
          
                    return Ok(new
                    {
                        bookingStartDate = booking.bookingStartDate,
                        bookingEndDate = booking.bookingEndDate,
                        bookingDestination = booking.bookingDestination,
                        
                      

                        paymentName = payment.paymentName,
                        paymentAmount = payment.paymentAmount,
                        paymentBalance = payment.paymentBalance,
                     //   paymentbookingId = payment.bookingId,

                    });

                    
                
            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        //put
        [HttpPut("{paymentId}")]
        public IActionResult Put(Int64 paymentId, [FromBody] decimal paymentAmount)
        {
            try
            {

                Payment payment = _unitOfWork.GetObjectByKey<Payment>(paymentId);
                if (payment != null)
                {
                    payment.paymentAmount = paymentAmount;
                    _unitOfWork.Save(payment);
                    _unitOfWork.CommitChanges();
                    return Ok(payment);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        //   put
        //[HttpPut("{paymentId}")]
        //public IActionResult Put(Int64 paymentId)
        //{
        //    try
        //    {

        //        Payment payment = _unitOfWork.GetObjectByKey<Payment>(paymentId);
        //        if (payment != null)
        //        {
        //            //    payment.paymentAmount = paymentAmount;
        //            // Create an instance of PaymentController
        //            //  var paymentController1 = new CarRentalManagement.Module.Controllers.PaymentController();
        //            // paymentController1.ProcessPayments(payment);

        //            _paymentController.ProcessPayments(payment);
        //            _unitOfWork.Save(payment);
        //            _unitOfWork.CommitChanges();
        //            return Ok(payment);
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}
    }
}
