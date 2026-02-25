using Clinic.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Clinic.Infrastructure.Identity;
using Clinic.Application.Interfaces;


namespace Clinic.Infrastructure.Data
{
	public class ClinicDbContext
		: IdentityDbContext<ApplicationUser, IdentityRole, string>
	{
		private readonly ICurrentUserService _currentUser;
		public ClinicDbContext(
		DbContextOptions<ClinicDbContext> options,
		ICurrentUserService currentUser)
		: base(options)
		{
			_currentUser = currentUser;
		}

		public DbSet<ClinicEntity> Clinics => Set<ClinicEntity>();
		public DbSet<Patient> Patients { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Example: Patient → Clinic relationship
			builder.Entity<Patient>()
				.HasOne(p => p.ClinicEntity)
				.WithMany(c => c.Patients)
				.HasForeignKey(p => p.ClinicEntityId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Patient>()
				.HasQueryFilter(p => p.ClinicEntityId == _currentUser.ClinicId);
		}
	}
}
