using pentasharp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.ViewModel.TaxiReservation
{
    public class TaxiReservationViewModel
    {

        public int ReservationId { get; set; }
        // User-selected start time for the trip
        [Required(ErrorMessage = "Please select a start date and time for the trip.")]
        public DateTime? TripStartTime { get; set; }

        // Automatically populated when the reservation is saved
        public DateTime ReservationTime { get; set; }

        // Other fields required for reservation
        public int TaxiCompanyId { get; set; }

        [Required(ErrorMessage = "Please enter a pickup location.")]
        public string PickupLocation { get; set; }

        [Required(ErrorMessage = "Please enter a dropoff location.")]
        public string DropoffLocation { get; set; }

        [Range(1, 4, ErrorMessage = "Passenger count must be between 1 and 4.")]
        public int PassengerCount { get; set; }

    }
}
