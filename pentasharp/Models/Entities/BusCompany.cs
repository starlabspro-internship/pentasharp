using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.Entities
{
    public class BusCompany
    {
        [Key]
        public int BusCompanyId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string ContactInfo { get; set; }

        [Required]  
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Buses> Buses { get; set; } = new List<Buses>();

    }
}
