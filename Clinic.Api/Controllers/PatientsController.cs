using Clinic.Application.DTOs.Patients;
using Clinic.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PatientsController : ControllerBase
	{
		private readonly IPatientService _patientService;

		public PatientsController(IPatientService patientService)
		{
			_patientService = patientService;
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreatePatientDto dto)
		{
			var id = await _patientService.CreateAsync(dto);
			return CreatedAtAction(nameof(Create), new { id }, null);
		}
	}
}
