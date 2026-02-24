using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Domain.Entities
{
	public class Patient
	{
		public int Id { get; set; }

		public int ClinicEntityId { get; set; }
		public ClinicEntity ClinicEntity { get; set; } = null!;

		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public DateTime DateOfBirth { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
