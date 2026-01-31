using Clinic.Application.DTOs.Patients;
using FluentValidation;

namespace Clinic.Application.Validators.Patients
{
	public class CreatePatientValidator : AbstractValidator<CreatePatientDto>
	{
		public CreatePatientValidator()
		{
			RuleFor(x => x.FirstName)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(x => x.LastName)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(x => x.DateOfBirth)
				.LessThan(DateTime.Today)
				.WithMessage("Date of birth must be in the past");
		}
	}
}
