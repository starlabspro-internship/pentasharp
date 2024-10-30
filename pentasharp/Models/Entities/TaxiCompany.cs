using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pentasharp.Models.Entities
{
	public class TaxiCompany
	{
		[Key]
		public int TaxiCompanyId { get; set; }

		[Required]
		[StringLength(100)]
		public string CompanyName { get; set; } = string.Empty;

		[Required]
		[StringLength(256)]
		public string ContactInfo { get; set; } = string.Empty;

		[Required]
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public DateTime? UpdatedAt { get; set; }

		public ICollection<Taxi> Taxis { get; set; } = new List<Taxi>();
	}
}