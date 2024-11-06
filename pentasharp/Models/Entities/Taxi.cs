using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using pentasharp.Models.Enums;

namespace pentasharp.Models.Entities
{
    public class Taxi
    {
        [Key]
        public int TaxiId { get; set; }

        [Required]
        [ForeignKey("TaxiCompany")]
        public int TaxiCompanyId { get; set; }

        [Required]
        [StringLength(20)]
        public string LicensePlate { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string DriverName { get; set; } = string.Empty;

        [Required]
        public TaxiStatus Status { get; set; } = TaxiStatus.Available;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public TaxiCompany TaxiCompany { get; set; } = null!;

        public ICollection<TaxiReservations> TaxiReservations { get; set; } = new List<TaxiReservations>();

        public ICollection<TaxiBookings> TaxiBookings { get; set; } = new List<TaxiBookings>();

    }
}
