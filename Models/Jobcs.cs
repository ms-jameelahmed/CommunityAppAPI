using CommunityAppAPI.Models;

namespace CommunityAppAPI.Models
{
    public class Job
    {
        public long JobId { get; set; }                // Auto-generated
        public long CustomerId { get; set; }           // Required
        public long ServiceId { get; set; }            // Required
        public string? Remarks { get; set; }           // Optional
        public string Status { get; set; }
        public bool Active { get; set; } = true;       // Required (default true)
        public DateTime ExpectedDate { get; set; }
        public string ContactNumber { get; set; }
        public string Priority { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool SiteVisitRequired { get; set; }

        // Navigation properties
        public List<JobMedia>? MediaList { get; set; }
        public List<JobDistribution>? Distributions { get; set; }
        public List<JobQuotationRequest>? JobQuotationRequest { get; set; }
        public List<JobQuotationResponse>? JobQuotationResponce { get; set; }
    }
    public class JobMedia
    {
        public long? IIdentity { get; set; }                // Auto-generated
        public long? JobId { get; set; }                    // Required
        public char From { get; set; }                     // 'C' (Customer) or 'V' (Vendor)
        public long CustomerId { get; set; }               // Required
        public long VendorId { get; set; }                 // Required
        public long UID { get; set; }                      // Auto-generated
        public char PhotoVideoType { get; set; }               // 'P' (Photo), 'V' (Video)
        
        public byte[] FileContent { get; set; } = null!;
        public char Type { get; set; }                     // 'B' (Before), 'A' (After)
        public long? InRefUID { get; set; }                // Optional
        public bool? Active { get; set; } = true;           // Required
        public string? CreatedBy { get; set; } = null!;
        public DateTime?  CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        
    }
    public class JobDistribution
    {
       
        public long? JobId { get; set; }                   // Required
        public long ServiceId { get; set; }               // Required
        public long VendorId { get; set; }                // Required
        public long CustomerId { get; set; }
        public bool? Active { get; set; } = true;          // Required
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Status { get; set; }
        public int? StatusId { get; set; }
        public string?  Name { get; set; }
        public long? SiteVisitId { get; set; }
        public bool? IsAcceptedByCustomer { get; set; }
    }
    public class JobStatusTrackingResponse
    {
        public IEnumerable<JobStatusTrackingDto> StatusTracking { get; set; }
        public IEnumerable<JobPartyInfoDto> PartyInfo { get; set; }
    }
    public class JobPartyInfoDto
    {
        public string CustomerName { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Mobile { get; set; }
        public string VendorName { get; set; }
        public decimal VendorLongitude { get; set; }
        public decimal VendorLatitude { get; set; }
        public string VendorMobile { get; set; }
        public string EmployeePhoneNumber { get; set; }
        public string EmployeeName { get; set; }
    }
    public class JobQuotationRequest
    {

        public long? JobId { get; set; }                   // Required
        public long ServiceId { get; set; }               // Required
        public long ToVendorId { get; set; }                // Required
        public string VendorName { get; set; }
        public long FromCustomerId { get; set; }
        public bool? Active { get; set; } = true;          // Required
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Status { get; set; }
        public long? QuotationId { get; set; }
        public long? QuotationResponseId { get; set; }
        public List<JobQuotationRequestItems> JobQuotationRequestItems { get; set; }
        public int? StatusId { get; set; }
        public bool? HasQuotationResponse { get; set; }
        public long? SiteVisitId { get; set; }
        public bool? IsAcceptedByCustomer { get; set; }
    }
    public class JobQuotationRequestItems
    { 
        public long VendorId { get; set; }   
    }
    public class JobQuotationResponse
    {
        public long? JobId { get; set; }
        public long? QuotationRequestId { get; set; }                   // Required
        public long ServiceId { get; set; }               // Required
        public long VendorId { get; set; }
        public string? VendorName { get; set; }
        public long? SiteVisitId { get; set; }
        public bool? IsAcceptedByCustomer { get; set; }
        public string? QuotationDetails { get; set; }
        public decimal? QuotationAmount { get; set; }
        public decimal? ServiceCharge { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public string? Status { get; set; }
        public long? QuotationResponceId { get; set; }
        public List<JobQuotationResponseItems> JobQuotationResponseItems { get; set; }
      
    }
    public class JobQuotationResponseItems
    {
        public long? QuotationResponceId { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class JobQuotationResponseAccept
    {
        public long? IIdentity { get; set; }
        public long? JobId { get; set; }
        public long? QuotationRequestId { get; set; }
        public long? QuotationResponseId { get; set; }    // Required
        public long VendorId { get; set; }
        public string? Remarks { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
    }

}
public class  JobDetailDto
{
    public long JobId { get; set; }
    public string CustomerName { get; set; }
    public string ServiceName { get; set; }
    public DateTime CustomerRequestedDate { get; set; }
    public decimal EstimatedAmount { get; set; }
    public string JobStatusCategory { get; set; }
    public string Status { get; set; }
    public List<AssignEmployeeItem> AssignedEmployees { get; set; } = new();
}

public class AssignEmployeeRequest
{
    public long JobId { get; set; }
    public long CustomerId { get; set; }
    public List<AssignEmployeeItem> AssignEmployeeList { get; set; }
}

public class AssignEmployeeItem
{
    public string EmployeeName { get; set; }
    public string EmployeePhoneNumber { get; set; }
    public string? EmiratesIdPhoto { get; set; }
    public string? EmiratesIdNumber { get; set; }
    public string? EmployeeEmail { get; set; }
}

public class AssignedEmployeeDto
{
    public string EmployeeName { get; set; }
    public string EmployeePhoneNumber { get; set; }
    public byte[]? EmiratesIdPhoto { get; set; }
    public string? EmiratesIdNumber { get; set; }
}

public class CustomerJobWithEmployeesDto
{
    public long JobId { get; set; }
    public string CustomerName { get; set; }
    public long CustomerId { get; set; }

    public string ServiceName { get; set; }
    public DateTime CustomerRequestedDate { get; set; }
    public decimal EstimatedAmount { get; set; }
    public string JobStatusCategory { get; set; }
    public string Status { get; set; }
    public string Address { get; set; }
    public List<AssignedEmployeeDto> AssignedEmployees { get; set; }
}

public class QuotationRequestWithResponseDto
{
    public long QuotationId { get; set; }
    public long? QuotationResponseId { get; set; }
    public long JobId { get; set; }
    public long FromCustomerId { get; set; }
    public long ToVendorId { get; set; }

    public string QuotationRequestStatus { get; set; }
    public string QuotationResponseStatus { get; set; }
    public string CustomerName { get; set; }
    public string CustomerMobile { get; set; }
    public string CustomerAddress { get; set; }
    public string ServiceName { get; set; }
    public DateTime ExpectedDate { get; set; }
    public int ServiceId { get; set; }

    public string Remarks { get; set; }
    public bool Deleted { get; set; }
    public bool Active { get; set; }
    public bool siteVisit { get; set; }
    public long? SiteVisitId { get; set; }
    public bool? IsAcceptedByCustomer { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public string StatusCategory { get; set; }
    public string Status  { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string ModifiedBy { get; set; }
    public decimal? QuotationAmount { get; set; }
}

public class JobPhotoPairDto
{
    public byte[] BeforePhoto { get; set; }
    public byte[] AfterPhoto { get; set; }
}

public class JobCompletionRequestDto
{
    public long JobId { get; set; }
    public List<JobPhotoPairDto> PhotoPairs { get; set; }
    public string Notes { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string CreatedBy { get; set; }
}

public class RequestedJobs
{
    public long JobId { get; set; }
    public string description { get; set; }
    public string status { get; set; } 
    public string ServiceName { get; set; }
    public DateTime ExpectedDate { get; set; }
    public int ServiceId { get; set; }

    public string Remarks { get; set; }
  
    public DateTime? CreatedDate { get; set; }
    public string CreatedBy { get; set; }
}
public class QuotationResponseDto
{
    public long QuotationResponseId { get; set; }
    public long JobId { get; set; }
    public long QuotationRequestId { get; set; }
    public long FromVendorId { get; set; }
    public string QuotationDetails { get; set; }
    public decimal QuotationAmount { get; set; }
    public decimal ServiceCharge { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool Deleted { get; set; }
    public bool Active { get; set; }

    public List<QuotationItemDto> Items { get; set; } = new();
}
public class QuotationItemDto
{
    public long ItemId { get; set; }
    public string Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalAmount { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}
public class JobInfoDetailDto
{
    public string CustomerName { get; set; }
    public string ServiceName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public DateTime ExpectedDate { get; set; }
    public string Priority { get; set; }
    public byte[] FileContent { get; set; } // This will be base64 encoded if returned in JSON
    public string Remarks { get; set; }
}

public class JobCompletionPhotoDto
{
    public byte[] BeforePhotoUrl { get; set; }
    public byte[] AfterPhotoUrl { get; set; }
    public bool IsBeforeVideo { get; set; }
    public bool IsAfterVideo { get; set; }
}

public class JobCompletionResponseDto
{
    public string Notes { get; set; }
    public List<JobCompletionPhotoDto> Photos { get; set; }
}

public class CustomerJobCompletionUpdateDto
{
    public long JobId { get; set; }
    public string Notes { get; set; }
    public int WorkDonePercentage { get; set; }
    public int Rating { get; set; }
    public string Feedback { get; set; }
    public string CreatedBy { get; set; }
}
public class  JobStatusUpdateDto
{
    public long JobId { get; set; }
    public string? Notes { get; set; }
    public int StatusId { get; set; }
    public int? VendorId { get; set; }
    
    public string Feedback { get; set; }
    public string CreatedBy { get; set; }
}
public class JobStatusTracking
{
    public long JobId { get; set; }
}

public class JobVendorHistoryTracking
{
    public long? JobId { get; set; }
    public int VendorId { get; set; }
}