using Microsoft.AspNetCore.Identity;
using Clinic.Domain.Entities;

namespace Clinic.Infrastructure.Identity
{
	public class ApplicationUser : IdentityUser
	{
		public int ClinicEntityId { get; set; }

		public ClinicEntity ClinicEntity { get; set; } = null!;
	}
}
