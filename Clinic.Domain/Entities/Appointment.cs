using Clinic.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Domain.Entities
{
	public class Appointment
	{
		public int Id { get; set; }

		public DateTime AppointmentDate { get; set; }

		public int PatientId { get; set; }
		public int DoctorId { get; set; }

		public AppointmentStatus Status { get; set; }

		// Navigation properties (for later EF Core use)
		public Patient Patient { get; set; } = null!;
		public Doctor Doctor { get; set; } = null!;
	}
}
