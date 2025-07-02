namespace CommunityAppAPI.Models
{
    public class Job
    {
        public long JobId { get; set; }                // Auto-generated
        public long CustomerId { get; set; }           // Required
        public long ServiceId { get; set; }            // Required
        public string? Remarks { get; set; }           // Optional
        public bool Active { get; set; } = true;       // Required (default true)
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public List<JobMedia>? MediaList { get; set; }
        public List<JobDistribution>? Distributions { get; set; }
    }
    public class JobMedia
    {
        public long IIdentity { get; set; }                // Auto-generated
        public long JobId { get; set; }                    // Required
        public char From { get; set; }                     // 'C' (Customer) or 'V' (Vendor)
        public long CustomerId { get; set; }               // Required
        public long VendorId { get; set; }                 // Required
        public long UID { get; set; }                      // Auto-generated
        public char PhotoVideo { get; set; }               // 'P' (Photo), 'V' (Video)
        public byte[] PhotoVideoContent { get; set; } = null!;
        public char Type { get; set; }                     // 'B' (Before), 'A' (After)
        public long? InRefUID { get; set; }                // Optional
        public bool Active { get; set; } = true;           // Required
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class JobDistribution
    {
        public long IIdentity { get; set; }               // Auto-generated
        public long JobId { get; set; }                   // Required
        public long ServiceId { get; set; }               // Required
        public long VendorId { get; set; }                // Required
        public bool Active { get; set; } = true;          // Required
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

}
