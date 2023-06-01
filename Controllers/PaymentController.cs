using System.Threading.Tasks;
using Dansnom.Payment;
using Dtos.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPayStackPayment _payStackPayment;

        public PaymentController(IPayStackPayment payStackPayment)
        {
            _payStackPayment = payStackPayment;
        }

        [HttpPost("InitiatePayment/{id}/{orderId}")]
        public async Task<IActionResult> MakePayMent([FromBody]CreatePaymentRequestModel model,[FromRoute] int id,[FromRoute] int orderId)
        {
            var payment = await _payStackPayment.InitiatePayment(model, id, orderId);
            return Ok(payment);
        }
        [HttpGet("Get/{transactionReference}")]
        public async Task<IActionResult> GetTransactionReceipt([FromRoute]string transactionReference)
        {
            var transaction = await PayStackPayment.GetTransactionRecieptAsync(transactionReference);
            if (transaction == null)
            {
                return BadRequest(transaction);
            }
            return Ok(transaction);
        }
    }
}