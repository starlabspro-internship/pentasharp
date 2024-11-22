using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;

namespace pentasharp.ViewModel.BusReservation
{
	public class AddBusReservationViewModel
	{

		public int ReservationId { get; set; }

		public DateTime ReservationDate { get; set; }

		public int NumberOfSeats { get; set; }

		public decimal TotalAmount { get; set; }

		public int ScheduleId { get; set; }

		public int UserId { get; set; }

        public BusReservationStatus Status { get; set; } = BusReservationStatus.Pending;

    }
}