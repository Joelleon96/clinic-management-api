using Clinic.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Clinic.Infrastructure.Identity;


namespace Clinic.Infrastructure.Data
{
	public class ClinicDbContext
		: IdentityDbContext<ApplicationUser, IdentityRole, string>
	{
		public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
			: base(options)
		{
		}

		public DbSet<ClinicEntity> Clinics => Set<ClinicEntity>();
		public DbSet<Patient> Patients { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Example: Patient → Clinic relationship
			builder.Entity<Patient>()
				.HasOne(p => p.ClinicEntity)
				.WithMany()
				.HasForeignKey(p => p.ClinicEntityId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
