using Clinic.Application.DTOs.Auth;
using Clinic.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Clinic.Infrastructure.Services;

public class AuthService : IAuthService
{
	private readonly IConfiguration _config;

	public AuthService(IConfiguration config)
	{
		_config = config;
	}

	public Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
	{
		// DEMO LOGIN
		if (request.Email != "admin@clinic.com" || request.Password != "123456")
			throw new UnauthorizedAccessException("Invalid credentials");

		var claims = new[]
		{
			new Claim(ClaimTypes.Email, request.Email),
			new Claim(ClaimTypes.Role, "Admin")
		};

		var key = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
		);

		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _config["Jwt:Issuer"],
			audience: _config["Jwt:Audience"],
			claims: claims,
			expires: DateTime.UtcNow.AddHours(2),
			signingCredentials: creds
		);

		return Task.FromResult(new LoginResponseDto
		{
			Token = new JwtSecurityTokenHandler().WriteToken(token)
		});
	}
}
