using AutoMapper;
using Clinic.Application.DTOs.Patients;
using Clinic.Domain.Entities;

namespace Clinic.Application.Mappings
{
	public class PatientProfile : Profile
	{
		public PatientProfile()
		{
			// DTO → Entity
			CreateMap<CreatePatientDto, Patient>();
			CreateMap<UpdatePatientDto, Patient>();

			// Entity → DTO
			CreateMap<Patient, PatientDto>();
		}
	}
}
