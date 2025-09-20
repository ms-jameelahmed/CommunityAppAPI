namespace CommunityAppAPI.Models
{
    public class CommunityMaster
    {
        public int CommunityId { get; set; }
        public string Name { get; set; }
        public bool? Deleted { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }

    public class DocumentMaster
    {
        public int DocumentTypeId { get; set; }
        public string DocumentName { get; set; }
        public string ForVendorCustomer { get; set; }
        public string Remarks { get; set; }
        public bool? Deleted { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
