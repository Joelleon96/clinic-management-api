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

		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetById(int id)
		{
			var patient = await _patientService.GetByIdAsync(id);

			if (patient == null)
				return NotFound();

			return Ok(patient);
		}

		[HttpPut("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var updated = await _patientService.UpdateAsync(id, dto);

			if (!updated)
				return NotFound();

			return NoContent();
		}

		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(int id)
		{
			var deleted = await _patientService.DeleteAsync(id);

			if (!deleted)
				return NotFound();

			return NoContent();
		}
	}
}
