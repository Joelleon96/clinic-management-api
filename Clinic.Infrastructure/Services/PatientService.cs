using Clinic.Application.DTOs.Patients;
using Clinic.Application.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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

		public async Task<IEnumerable<PatientDto>> GetAllAsync()
		{
			return await _context.Patients
				.Select(p => new PatientDto
				{
					Id = p.Id,
					FirstName = p.FirstName,
					LastName = p.LastName,
					DateOfBirth = p.DateOfBirth
				})
				.ToListAsync();
		}

		public async Task<PatientDto?> GetByIdAsync(int id)
		{
			var patient = await _context.Patients
				.Where(p => p.Id == id)
				.Select(p => new PatientDto
				{
					Id = p.Id,
					FirstName = p.FirstName,
					LastName = p.LastName,
					DateOfBirth = p.DateOfBirth
				})
				.FirstOrDefaultAsync();

			return patient;
		}
		public async Task<bool> UpdateAsync(int id, UpdatePatientDto dto)
		{
			var patient = await _context.Patients
				.FirstOrDefaultAsync(p => p.Id == id);

			if (patient == null)
				return false;

			patient.FirstName = dto.FirstName;
			patient.LastName = dto.LastName;
			patient.DateOfBirth = dto.DateOfBirth;

			await _context.SaveChangesAsync();
			return true;
		}

	}
}
