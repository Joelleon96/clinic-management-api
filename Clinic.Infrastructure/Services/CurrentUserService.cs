using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Clinic.Application.Interfaces;

public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CurrentUserService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string? UserId =>
		_httpContextAccessor.HttpContext?.User?
			.FindFirst(ClaimTypes.NameIdentifier)?.Value;

	public int ClinicId
	{
		get
		{
			var clinicClaim = _httpContextAccessor.HttpContext?.User?
				.FindFirst("clinicId")?.Value;

			return clinicClaim != null ? int.Parse(clinicClaim) : 0;
		}
	}
}