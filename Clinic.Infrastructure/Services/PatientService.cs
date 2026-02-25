using AutoMapper;
using Clinic.Application.DTOs.Common.Clinic.Application.DTOs.Common;
using Clinic.Application.DTOs.Patients;
using Clinic.Application.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Services
{
	public class PatientService : IPatientService
	{
		public async Task<PagedResult<PatientDto>> GetAllAsync(PaginationQuery query)
		{
			var baseQuery = _context.Patients.AsNoTracking();

			var totalCount = await baseQuery.CountAsync();

			var patients = await baseQuery
				.Skip((query.PageNumber - 1) * query.PageSize)
				.Take(query.PageSize)
				.ToListAsync();

			return new PagedResult<PatientDto>
			{
				Items = _mapper.Map<IEnumerable<PatientDto>>(patients),
				TotalCount = totalCount,
				PageNumber = query.PageNumber,
				PageSize = query.PageSize
			};
		}
		private readonly ClinicDbContext _context;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUser;

		public PatientService(
			ClinicDbContext context,
			IMapper mapper,
			ICurrentUserService currentUser)
		{
			_context = context;
			_mapper = mapper;
			_currentUser = currentUser;
		}

		public async Task<int> CreateAsync(CreatePatientDto dto)
		{
			var patient = _mapper.Map<Patient>(dto);

			patient.CreatedAt = DateTime.UtcNow;

			
			patient.ClinicEntityId = _currentUser.ClinicId;

			_context.Patients.Add(patient);
			await _context.SaveChangesAsync();

			return patient.Id;
		}

		public async Task<PatientDto?> GetByIdAsync(int id)
		{
			var patient = await _context.Patients.FindAsync(id);

			if (patient == null)
				throw new Exception("Patient not found");

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
