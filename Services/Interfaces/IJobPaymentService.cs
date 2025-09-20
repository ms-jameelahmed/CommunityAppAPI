using CommunityAppAPI.Models;

namespace CommunityAppAPI.Services.Interfaces
{
    public interface IJobPaymentService
    {
        Task<long> CreatePaymentAsync(JobPaymentDetailsDto dto, string createdBy);
        Task<IEnumerable<JobPaymentDetailsDto>> GetPaymentsAsync(long? paymentIdentity, long? jobId, string invoiceNumber);
        Task<InvoiceDetailDto> GetInvoiceDetailAsync(long? VendorId, long? jobId);
    }

}
