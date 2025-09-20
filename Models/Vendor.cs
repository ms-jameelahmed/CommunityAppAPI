using CommunityAppAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;

public class Vendor
{

    public long CustomerId { get; set; }

    [Required]
    public int TypeId { get; set; }

    [Required]
    public char Type { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(25)]
    public string Mobile { get; set; }

    [MaxLength(25)]
    public string Landline { get; set; }

    [MaxLength(25)]
    public string? AlternateContactNo { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; }

    public int? CommunityId { get; set; }

    [MaxLength(50)]
    public string Building { get; set; }

    [MaxLength(20)]
    public string Block { get; set; }

    [Required]
    [MaxLength(255)]
    public string Address { get; set; }

    [MaxLength(50)]
    public string Latitude { get; set; }

    [MaxLength(50)]
    public string Longitude { get; set; }

    public bool? Blacklisted { get; set; }

    public decimal? SettlementPercentage { get; set; }

    public bool Deleted { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedDate { get; set; }

    [Required]
    [MaxLength(50)]
    public string CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [MaxLength(50)]
    public string? ModifiedBy { get; set; }
    // 👇 Login Details Section
    [Required]
    [MaxLength(30)]
    public string UserId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Password { get; set; }  // Make sure this is hashed before saving

    public bool LoginEnable { get; set; }

    [Required]
    public char CustomerType { get; set; }
    public BankDetail BankDetail { get; set; }
    public List<VendorDocument> Documents { get; set; } = new();

    public byte[]? Image { get; set; }
}

public class BankDetail
{
    public long? IIdentity { get; set; }
    public long? VendorId { get; set; }

    public string BankName { get; set; }
    public string AccountNumber { get; set; }
    public string? IBAN { get; set; }
    public string? SWIFTBIC { get; set; }
    public string BankBranch { get; set; }
    public string Address { get; set; }

    public bool Active { get; set; }
    public bool Deleted { get; set; }

    public DateTime? CreatedDate { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
   
}

public class VendorDocument
{
    public long? DocumentIdentity { get; set; }
    public long? VendorId { get; set; }
    public int DocumentTypeId { get; set; }
    public string? DocumentType { get; set; }
    public string? DocumentNumber { get; set; }
    public DateTime DocumentExpiryDate { get; set; }
    public byte[] DocumentFile { get; set; }

    public bool Active { get; set; }
    public bool Deleted { get; set; }

    public DateTime? CreatedDate { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
}


public class Vendor_Service
{
    public int IIdentity { get; set; }
    public int VendorId { get; set; }
    public string? VendorName { get; set; }
    public int ServiceId { get; set; }
    public int? Service  { get; set; }
    public bool Deleted { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public string? Description { get; set; }
    public byte[]? Image { get; set; }
    public string? Address { get; set; }
    public string? Rating { get; set; }
    public string? reviewCount {  get; set; }
    public string? averageResponseTime { get; set; }
}
 
public class QuotationSummaryDto
{
    public int Pending { get; set; }
    public int Accepted { get; set; }
    public int Rejected { get; set; }

}
public class CustomerSummaryDto
{
    public int ongoing { get; set; }
    public int completed { get; set; }
     

}

public class JobStatusCategoryDto
{
    public int Count { get; set; }
    public string Category { get; set; }
}

public class PromotionalContentDto
{
    public int Id { get; set; }
    public string Image { get; set; }          // URL or image path
    public string Title { get; set; }          // Title of the promotion
    public string Description { get; set; }    // Longer text (optional)
    public string PromoCode { get; set; }      // Promotional code
    public DateTime ValidTill { get; set; }    // Expiry date of the promotion
    public DateTime CreatedDate { get; set; }  // Date it was created
    public bool Active { get; set; }           // Indicates if the promotion is active
}
public class CustomerDashboardDto
{
    
    public CustomerSummaryDto jobstats { get; set; }
    public List<PromotionalContentDto>? Promotionalcontent { get; set; }
}

public class VendorSummaryDto
{
    public int Pending { get; set; }
    public int Accepted { get; set; }
    public int Rejected { get; set; }

    public int newRequest { get; set; }
    public int awaitingQuotation { get; set; }
    public int inProgress { get; set; }
    public int pendingPayment { get; set; }

    public int Rating { get; set; }
}

 
public class VendorDashboardDto
{
    public List<ServiceMaster> ServiceMaster { get; set; }
    public List<VendorDocument> Documents { get; set; }
    public VendorSummaryDto Stats { get; set; }
    public List<JobStatusCategoryDto> JobStatusCounts { get; set; }
}