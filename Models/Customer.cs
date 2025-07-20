using System.ComponentModel.DataAnnotations;

namespace CommunityAppAPI.Models
{
    public class Customer
    {
        public long? CustomerId { get; set; }

        
        public int  TypeId { get; set; }

        [Required]
        public char Type { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(25)]
        public string Mobile { get; set; }

        [MaxLength(25)]
        public string? Landline { get; set; }

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

        public DateTime? CreatedDate { get; set; }

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

        public byte[]? Image { get; set; }
    }

    public class ServiceMaster
    {
        public long ServiceId { get; set; }
        public string ServiceName { get; set; }
        public bool? Deleted { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
