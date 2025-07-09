using System.ComponentModel.DataAnnotations;

namespace CommunityAppAPI.DTOs
{
    public class AuthResponseDto
    {
        public string Email { get; set; }
        public string Token { get; set; }

        public long CustomerId { get; set; }

        
        public int TypeId { get; set; }

        
        public char Type { get; set; }

        
        [MaxLength(50)]
        public string Name { get; set; }

        
        [MaxLength(25)]
        public string Mobile { get; set; }

        [MaxLength(25)]
        public string Landline { get; set; }

        [MaxLength(25)]
        public string AlternateContactNo { get; set; }

       

        public int? CommunityId { get; set; }

        [MaxLength(50)]
        public string Building { get; set; }

        [MaxLength(20)]
        public string Block { get; set; }

        
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

        // 👇 Login Details Section
        
        [MaxLength(30)]
        public string UserId { get; set; }

        
        [MaxLength(150)]
        public string Password { get; set; }  // Make sure this is hashed before saving

        
        public bool LoginEnable { get; set; }

        
        public char CustomerType { get; set; }

    }
}
