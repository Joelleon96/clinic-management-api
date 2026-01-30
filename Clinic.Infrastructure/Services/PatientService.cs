using AutoMapper;
using Clinic.Application.DTOs.Patients;
using Clinic.Application.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Services
{
	public class PatientService : IPatientService
	{
		private readonly ClinicDbContext _context;
		private readonly IMapper _mapper;

		public PatientService(ClinicDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<int> CreateAsync(CreatePatientDto dto)
		{
			var patient = _mapper.Map<Patient>(dto);

			patient.CreatedAt = DateTime.UtcNow;

			_context.Patients.Add(patient);
			await _context.SaveChangesAsync();

			return patient.Id;
		}

		public async Task<PatientDto?> GetByIdAsync(int id)
		{
			var patient = await _context.Patients.FindAsync(id);

			if (patient == null)
				return null;

			return _mapper.Map<PatientDto>(patient);
		}

		public async Task UpdateAsync(int id, UpdatePatientDto dto)
		{
			var patient = await _context.Patients.FindAsync(id);

			if (patient == null)
				throw new Exception("Patient not found");

			_mapper.Map(dto, patient);

			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var patient = await _context.Patients.FindAsync(id);

			if (patient == null)
				throw new Exception("Patient not found");

			_context.Patients.Remove(patient);
			await _context.SaveChangesAsync();
		}
	}
}
