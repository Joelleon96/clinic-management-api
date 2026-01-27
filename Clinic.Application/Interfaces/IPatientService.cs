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
	}
}
