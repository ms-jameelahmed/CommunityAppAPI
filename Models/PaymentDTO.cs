namespace CommunityAppAPI.Models
{
    public class PaymentDTO
    {
    }
    public class JobPaymentDetailsDto
    {
        public long PaymentIdentity { get; set; }
        public long JobId { get; set; }
        public long QuotationId { get; set; }
        public long AmountFrom { get; set; }
        public long AmountTo { get; set; }
        public string Type { get; set; }
        public decimal? CommPercentage { get; set; }
        public decimal? CommAmount { get; set; }
        public decimal Amount { get; set; }
        public string Mode { get; set; }
        public string PaymentTowards { get; set; }
        public string Remarks { get; set; }
        public string InvoiceNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string ReferenceType { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionStatus { get; set; }
    }
    public class InvoiceDetailDto
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public VendorDto Vendor { get; set; }
        public CompanyDto Company { get; set; }
        public CustomerDto Customer { get; set; }
        public JobDto Job { get; set; }
        public List<InvoiceLineItemDto> LineItems { get; set; }
        public InvoiceTotalsDto Totals { get; set; }
    }

    public class VendorDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TRN { get; set; }
    }

    public class CompanyDto
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public BankDetailsDto BankDetails { get; set; }
    }

    public class BankDetailsDto
    {
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }
    }

    public class CustomerDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class JobDto
    {
        public string ServiceName { get; set; }
        public string JobRef { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
    }

    public class InvoiceLineItemDto
    {
        public int SrNo { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public decimal? Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal Vat { get; set; }
        public decimal Total { get; set; }
    }

    public class InvoiceTotalsDto
    {
        public decimal SubTotal { get; set; }
        public decimal VatTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public string VatInWords { get; set; }
        public string TotalInWords { get; set; }
    }

}
