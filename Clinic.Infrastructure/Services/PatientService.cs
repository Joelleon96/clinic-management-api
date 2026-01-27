using Clinic.Application.DTOs.Patients;
using Clinic.Application.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.Services
{
	public class PatientService : IPatientService
	{
		private readonly ClinicDbContext _context;

		public PatientService(ClinicDbContext context)
		{
			_context = context;
		}

		public async Task<int> CreateAsync(CreatePatientDto dto)
		{
			var patient = new Patient
			{
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				DateOfBirth = dto.DateOfBirth,
				CreatedAt = DateTime.UtcNow
			};

			_context.Patients.Add(patient);
			await _context.SaveChangesAsync();

			return patient.Id;
		}
	}
}
