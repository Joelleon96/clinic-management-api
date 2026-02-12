using Clinic.Application.DTOs.Common;
using Clinic.Application.DTOs.Common.Clinic.Application.DTOs.Common;
using Clinic.Application.DTOs.Patients;
using Clinic.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class PatientsController : ControllerBase
	{


		private readonly IPatientService _patientService;

		public PatientsController(IPatientService patientService)
		{
			_patientService = patientService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
		{
			var result = await _patientService.GetAllAsync(query);
			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreatePatientDto dto)
		{
			var id = await _patientService.CreateAsync(dto);
			return CreatedAtAction(nameof(GetById), new { id }, null);
		}

		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var patient = await _patientService.GetByIdAsync(id);

			if (patient == null)
				return NotFound();

			return Ok(patient);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, UpdatePatientDto dto)
		{
			await _patientService.UpdateAsync(id, dto);
			return NoContent();
		}


		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _patientService.DeleteAsync(id);
			return NoContent();
		}
	}
}
