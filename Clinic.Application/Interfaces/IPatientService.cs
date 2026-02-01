using Clinic.Application.DTOs.Common.Clinic.Application.DTOs.Common;
using Clinic.Application.DTOs.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Interfaces
{
	public interface IPatientService
	{
		Task<int> CreateAsync(CreatePatientDto dto);
		Task<PatientDto?> GetByIdAsync(int id);
		Task<PagedResult<PatientDto>> GetAllAsync(PaginationQuery query);
		Task UpdateAsync(int id, UpdatePatientDto dto);
		Task DeleteAsync(int id);
	}
}
