using CommunityAppAPI.Models;
using CommunityAppAPI.Services;
using CommunityAppAPI.Services.Common;
using CommunityAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommunityAppAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class JobPaymentController : ControllerBase
    {
        private readonly IJobPaymentService _service;
        private readonly ICommonService _commonService;
        private readonly IResponseService _responseService;
        public JobPaymentController(IJobPaymentService service, IResponseService responseService, ICommonService commonService)
        {
            _responseService = responseService;
            _commonService = commonService;
            _service = service;
        }

        [HttpPost("CreatePayment")]
        
        public async Task<IActionResult> Create([FromBody] JobPaymentDetailsDto dto)
        {
            var createdBy = User.Identity?.Name ?? "system";
            var id = await _service.CreatePaymentAsync(dto, createdBy);
            return Ok(new { PaymentIdentity = id, Message = "Payment record created successfully" });
        }

        [HttpGet]
        
        public async Task<IActionResult> Get([FromQuery] long? paymentIdentity, [FromQuery] long? jobId, [FromQuery] string invoiceNumber)
        {
            var result = await _service.GetPaymentsAsync(paymentIdentity, jobId, invoiceNumber);
            return Ok(result);
        }

        
        
        [HttpPost("GetPaymentdetails")]
        public async Task<IActionResult>  GetPaymentdetails(JobpaymentTracking jobCustomer)
        {
            var invoice = await _service.GetInvoiceDetailAsync(jobCustomer.VendorId, jobCustomer.JobId);
            if (invoice == null)
                return NotFound(new { Message = "Invoice not found" });

            return Ok(invoice);
        }
    }
}
