using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using CommunityAppAPI.Services.Interfaces;

namespace CommunityAppAPI.Services
{
    public class JobPaymentService : IJobPaymentService
    {
        private readonly IJobPaymentRepository _repo;

        public JobPaymentService(IJobPaymentRepository repo)
        {
            _repo = repo;
        }

        public async Task<long> CreatePaymentAsync(JobPaymentDetailsDto dto, string createdBy)
        {
            return await _repo.InsertAsync(dto, createdBy);
        }

        public async Task<IEnumerable<JobPaymentDetailsDto>> GetPaymentsAsync(long? paymentIdentity, long? jobId, string invoiceNumber)
        {
            return await _repo.GetAsync(paymentIdentity, jobId, invoiceNumber);
        }

        public async Task<InvoiceDetailDto> GetInvoiceDetailAsync(long? VendorId, long? jobId)
        {
            return await _repo.GetInvoiceDetailAsync(VendorId,jobId);
        }
    }

}
