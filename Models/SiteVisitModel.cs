namespace CommunityAppAPI.Models
{
    public class AssignSiteVisitEmployeeRequest
    {
        public long JobId { get; set; }
        public long CustomerId { get; set; }
        public long SiteVisitId { get; set; }
        public List<AssignEmployeeItem> AssignEmployeeList { get; set; }
    }
    public class SiteVisitRequestDto
    {
        public long JobId { get; set; }
        public string RequestedBy { get; set; } // Vendor / Customer
        public string RequestedTo { get; set; } // Vendor / Customer
        public string CreatedBy { get; set; }
        public long VendorId { get; set; }
        public DateTime RequestedDate { get; set; }
        public long CustomerId { get; set; }
    }

    public class SiteVisitResponseDto
    {
        public long SiteVisitId { get; set; }
        public string Status { get; set; }
        public string QRCode { get; set; }
    }

    public class EmployeeAssignRequestDto
    {
        public long SiteVisitId { get; set; }
        public long EmployeeId { get; set; }
    }

    public class EmployeeInfoDto
    {
        public string EmployeeName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmiratesId { get; set; }
        public string Email { get; set; }
    }
    public class SiteVisitDetailDto
    {
        public long SiteVisitId { get; set; }
        public long JobId { get; set; }
        public long CustomerId { get; set; }
        public long VendorId { get; set; }
        public long? AssignedEmployeeId { get; set; }
        public string QrCode { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public bool? IsAcceptedByCustomer { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // Employees list
        public List<AssignedEmployeeDto> Employees { get; set; } = new();
    }

    public class GatePassRequest
    {
        public int RequestId { get; set; }
        public string VillaNumber { get; set; }
        public string ReasonForVisit { get; set; }
        public string TradeLicense { get; set; }
        public string VisitorName { get; set; }
        public string EmiratesId { get; set; }
        public byte[] EmiratesIdCopy { get; set; }
        public string VisitorMobile { get; set; }
        public string VisitorEmail { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string VendorEmail { get; set; }
    }


}
