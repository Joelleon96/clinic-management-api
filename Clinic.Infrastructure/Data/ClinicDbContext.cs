using Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Data
{
	public class ClinicDbContext : DbContext
	{
		public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
			: base(options)
		{
		}

		public DbSet<Patient> Patients => Set<Patient>();
	}
}
