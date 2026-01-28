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
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]

		public async Task<IActionResult> Create([FromBody] CreatePatientDto dto)
		{
			var id = await _patientService.CreateAsync(dto);
			return CreatedAtAction(nameof(Create), new { id }, new { id });

		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var patients = await _patientService.GetAllAsync();
			return Ok(patients);
		}


	}
}
