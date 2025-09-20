namespace CommunityAppAPI.Models
{
    public class HistoryDetailDto
    {
        public JobHistoryDto JobDetail { get; set; }
        public List<JobCompletionDetailDto> CompletionDetails { get; set; }
    }

    public class  HistoryDto
    {
        public long JobId { get; set; }
        public string CustomerName { get; set; }
        public string ServiceName { get; set; }
        public string Address { get; set; }
        public decimal TotalAmount { get; set; }
        public string FeedbackComments { get; set; }
        public int? Rating { get; set; }
        public DateTime JobDate { get; set; }
    }

    public class JobCompletionDetailDto
    {
        public string Notes { get; set; }
        public byte[] BeforePhotoUrl { get; set; }
        public byte[] AfterPhotoUrl { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePhoneNumber { get; set; }
    }
    public class VendorHistoryListDto
    {
        public long JobId { get; set; }
        public string CustomerName { get; set; }
        public string ServiceName { get; set; }
        public string Address { get; set; }
        public decimal QuotedAmount { get; set; }
        public DateTime Date { get; set; }
        public string Feedback { get; set; }
    }
    public class CustomerHistoryListDto
    {
        public long JobId { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string Priority { get; set; }
        public string VendorName { get; set; }
        public string Remarks { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string AssignedEmployee { get; set; }
        public string EmployeePhoneNumber { get; set; }
        public int? Rating { get; set; }
        public string FeedbackComments { get; set; }
        public string ServiceName { get; set; }
        public int ServiceId { get; set; }
        public int VendorId { get; set; }

    }
    public class CustomerHistoryDetails
    {
        public long JobId { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string Priority { get; set; }
        public string VendorName { get; set; }
        public string VendorPhoneNumber { get; set; }
        public string Remarks { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePhoneNumber { get; set; }
        public string FeedbackComments { get; set; }
        public string AssignedEmployee { get; set; }
        public int? Rating { get; set; }
      

    }
    public class CustomerHistoryDetailDto
    {
        public CustomerHistoryDetails JobDetail { get; set; }
        public List<JobCompletionDetailDto> CompletionDetails { get; set; }
    }
    public class JobCustomerHistoryTracking
    {
        public long CustomerId { get; set; }
        public long? JobId { get; set; }
    }
    public class JobpaymentTracking
    {
        public long VendorId { get; set; }
        public long? JobId { get; set; }
    }
    public class JobServiceDetail
    {
        public long VendorId { get; set; }
        public long ServiceId { get; set; }
    }
    public class ServiceDetailDto
    {
        public long VendorId { get; set; }
        public string ServiceName { get; set; }
        public byte[] ServiceImage { get; set; }
        public List<VendorDetailDto> Vendors { get; set; } = new();
        public List<CustomerReviewDto> Reviews { get; set; } = new();
        public List<CustomerReviewCount> ReviewCount { get; set; } = new();

    }

    public class VendorDetailDto
    {
        public long VendorId { get; set; }
        public string VendorName { get; set; }
        public byte[] VendorLogo { get; set; }
        public string Description { get; set; }
        public string VendorEmail { get; set; }
        public string Address { get; set; }
        public decimal TotalRating { get; set; }
        public int ReviewCount { get; set; }
        public string ServiceName { get; set; }
        public List<CustomerReviewDto> Reviews { get; set; } = new();

    }
    public class CustomerReviewCount
    {
 
        public decimal Rating { get; set; }
        public int Count { get; set; }
       
    }
    public class CustomerReviewDto
    {
        public long JobId { get; set; }
        public string CustomerName { get; set; }
        public byte[] CustomerImage { get; set; }
        public decimal Rating { get; set; }
        public string Feedback { get; set; }
        public DateTime ReviewDate { get; set; }
    }
    public class ExploreServiceDto
    {
        public long ServiceId { get; set; }
        public string ServiceName { get; set; }
        public byte[] ServiceImage { get; set; }
        public string VendorName { get; set; }
        public byte[] VendorLogo { get; set; }
        public decimal Rating { get; set; }
        public int ReviewCount { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string? CategoryName { get; set; }
        public long VendorId { get; set; }
    }
    public class EmailResponseDto
    {
        public string From { get; set; }
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
    public class CustomerResponseDto
    {
        public int jobid { get; set; }
        public int siteVisitId { get; set; }
        public bool isAccepted { get; set; }
    }
}
