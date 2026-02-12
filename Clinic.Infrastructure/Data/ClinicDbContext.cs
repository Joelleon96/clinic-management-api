using Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Clinic.Infrastructure.Data
{
	public class ClinicDbContext : DbContext
	{
		public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
			: base(options)
		{
		}

		public DbSet<Patient> Patients => Set<Patient>();
		public DbSet<User> Users { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>().HasData(
				new User
				{
					Id = 1,
					Email = "admin@clinic.com",
					PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
					Role = "Admin"
				}
			);
		}
	}
}
