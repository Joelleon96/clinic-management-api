using Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.Data
{
	public class ClinicDbContext : DbContext
	{
		public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
			: base(options) { }

		public DbSet<Patient> Patients => Set<Patient>();
		public DbSet<Doctor> Doctors => Set<Doctor>();
		public DbSet<Appointment> Appointments => Set<Appointment>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Future Fluent API configs go here
		}
	}
}
