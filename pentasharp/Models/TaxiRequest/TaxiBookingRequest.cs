using pentasharp.Models.Enums;
using System;

namespace pentasharp.Models.TaxiRequest
{
    public class TaxiBookingRequest
    {
        public int TaxiCompanyId { get; set; }
        public string PickupLocation { get; set; } = string.Empty;
        public string DropoffLocation { get; set; } = string.Empty;
        public DateTime BookingTime { get; set; }
        public int PassengerCount { get; set; }
        public int UserId { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    }
}