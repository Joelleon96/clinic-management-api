namespace Clinic.Domain.Entities
{
	public class ClinicEntity
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Address { get; set; } = string.Empty;

		public string PhoneNumber { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		// Navigation Properties
		public ICollection<Patient> Patients { get; set; }
			= new List<Patient>();
	}
}
