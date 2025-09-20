using CommunityAppAPI.Models;
using CommunityAppAPI.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CommunityAppAPI.Repositories
{
    public class JobPaymentRepository : IJobPaymentRepository
    {
        private readonly IDbConnection _connection;

        public JobPaymentRepository(IConfiguration config)
        {
            _connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<long> InsertAsync(JobPaymentDetailsDto dto, string createdBy)
        {
            try
            {

          
            var parameters = new DynamicParameters();
            parameters.Add("@JobId", dto.JobId);
            parameters.Add("@QuotationId", dto.QuotationId);
            parameters.Add("@AmountFrom", dto.AmountFrom);
            parameters.Add("@AmountTo", dto.AmountTo);
            parameters.Add("@Type", dto.Type);
            parameters.Add("@CommPercentage", dto.CommPercentage);
            parameters.Add("@CommAmount", dto.CommAmount);
            parameters.Add("@Amount", dto.Amount);
            parameters.Add("@Mode", dto.Mode);
            parameters.Add("@PaymentTowards", dto.PaymentTowards);
            parameters.Add("@Remarks", dto.Remarks);
            parameters.Add("@CreatedBy", createdBy);
            parameters.Add("@InvoiceNumber", dto.InvoiceNumber);
            parameters.Add("@ReferenceNumber", dto.ReferenceNumber);
            parameters.Add("@ReferenceType", dto.ReferenceType);
            parameters.Add("@TransactionDate", dto.TransactionDate);
            parameters.Add("@TransactionStatus", dto.TransactionStatus);
            parameters.Add("@PaymentIdentity", dbType: DbType.Int64, direction: ParameterDirection.Output);

            await _connection.ExecuteAsync("usp_JobPaymentDetails_Insert", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<long>("@PaymentIdentity");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<JobPaymentDetailsDto>> GetAsync(long? paymentIdentity, long? jobId, string invoiceNumber)
        {
            return await _connection.QueryAsync<JobPaymentDetailsDto>(
                "usp_JobPaymentDetails_Get",
                new { PaymentIdentity = paymentIdentity, JobId = jobId, InvoiceNumber = invoiceNumber },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<InvoiceDetailDto> GetInvoiceDetailAsync(long? vendorId, long? jobId)
        {
            using var multi = await _connection.QueryMultipleAsync(
                "usp_GetInvoiceDetail",
                new { VendorId = vendorId,JobId=jobId },
                commandType: CommandType.StoredProcedure);

            var header = await multi.ReadFirstOrDefaultAsync<dynamic>();
            var items = (await multi.ReadAsync<InvoiceLineItemDto>()).ToList();
            var totals = await multi.ReadFirstOrDefaultAsync<InvoiceTotalsDto>();

            if (header == null) return null;

            return new InvoiceDetailDto
            {
                InvoiceNumber = header.InvoiceNumber,
                InvoiceDate = header.InvoiceDate,
                Vendor = new VendorDto
                {
                    Name = header.VendorName,
                    Address = header.VendorAddress,
                    Email = header.VendorEmail,
                    Phone = header.VendorPhone,
                    TRN = header.VendorTRN
                },
                Company = new CompanyDto
                {
                    Name = header.CompanyName,
                    LogoUrl = header.LogoUrl,
                    BankDetails = new BankDetailsDto
                    {
                        BankName = header.BankName,
                        AccountName = header.AccountName,
                        AccountNumber = header.AccountNumber,
                        IBAN = header.IBAN
                    }
                },
                Customer = new CustomerDto
                {
                    Name = header.CustomerName,
                    Address = header.CustomerAddress,
                    Email = header.CustomerEmail,
                    Phone = header.CustomerPhone
                },
                Job = new JobDto
                {
                    ServiceName = header.ServiceName,
                    JobRef = header.JobRef,
                    RequestedDate = header.RequestedDate,
                    CompletedDate = header.CompletedDate
                },
                LineItems = items,
                Totals = new InvoiceTotalsDto
                {
                    SubTotal = totals.SubTotal,
                    VatTotal = totals.VatTotal,
                    GrandTotal = totals.GrandTotal,
                    VatInWords = ConvertToWords(totals.VatTotal),
                    TotalInWords = ConvertToWords(totals.GrandTotal)
                }
            };
        }

        private string ConvertToWords(decimal amount)
        {
            // You can implement a number-to-words converter here.
            return $"{amount} AED";
        }
    }

}
