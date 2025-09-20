using CommunityAppAPI.Models;

namespace CommunityAppAPI.Repositories.Interfaces
{
    public interface IJobPaymentRepository
    {
        Task<long> InsertAsync(JobPaymentDetailsDto dto, string createdBy);
        Task<IEnumerable<JobPaymentDetailsDto>> GetAsync(long? paymentIdentity, long? jobId, string invoiceNumber);

        Task<InvoiceDetailDto> GetInvoiceDetailAsync(long? VendorId, long? jobId);
    }
}
