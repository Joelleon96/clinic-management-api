using Clinic.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Clinic.Infrastructure.Identity;
using Clinic.Application.Interfaces;
using Clinic.Domain.Common;


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

		public override async Task<int> SaveChangesAsync(
		CancellationToken cancellationToken = default)
		{
			var entries = ChangeTracker
				.Entries<BaseAuditableEntity>();

			foreach (var entry in entries)
			{
				if (entry.State == EntityState.Added)
				{
					entry.Entity.CreatedAt = DateTime.UtcNow;
					entry.Entity.CreatedByUserId = _currentUser.UserId;
				}

				if (entry.State == EntityState.Modified)
				{
					entry.Entity.UpdatedAt = DateTime.UtcNow;
					entry.Entity.UpdatedByUserId = _currentUser.UserId;
				}
			}

			return await base.SaveChangesAsync(cancellationToken);
		}
		public DbSet<ClinicEntity> Clinics => Set<ClinicEntity>();
		public DbSet<Patient> Patients { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Example: Patient → Clinic relationship
			builder.Entity<Patient>()
			.HasQueryFilter(p =>
			p.ClinicEntityId == _currentUser.ClinicId &&
			!p.IsDeleted);

			builder.Entity<Patient>()
				.HasQueryFilter(p => p.ClinicEntityId == _currentUser.ClinicId);
		}
	}
}
