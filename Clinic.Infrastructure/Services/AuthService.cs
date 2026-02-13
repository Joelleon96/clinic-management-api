using Clinic.Application.DTOs.Auth;
using Clinic.Application.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Clinic.Infrastructure.Services;

public class AuthService : IAuthService
{
	private readonly ILogger<AuthService> _logger;
	private readonly IConfiguration _config;
	private readonly ClinicDbContext _context;

	public AuthService(IConfiguration config, ClinicDbContext context, ILogger<AuthService> logger)
	{
		_config = config;
		_context = context;
		_logger = logger;
	}

	public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
	{
		// 1️⃣ Find user by email
		var user = await _context.Users
			.FirstOrDefaultAsync(u => u.Email == request.Email);

		if (user == null)
		{
			_logger.LogWarning("Login failed: User not found for email {Email}", request.Email);
			throw new UnauthorizedAccessException("Invalid credentials");
		}

		// 2️⃣ Verify password using BCrypt
		bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
			request.Password,
			user.PasswordHash
		);

		if (!isPasswordValid)
		{
			_logger.LogWarning("Login failed: Invalid password for email {Email}", request.Email);
			throw new UnauthorizedAccessException("Invalid credentials");
		}

		// 3️⃣ Create claims
		var claims = new[]
		{
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Role, user.Role)
		};

		// 4️⃣ Generate JWT key
		var key = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
		);

		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		// 5️⃣ Create token
		var token = new JwtSecurityToken(
			issuer: _config["Jwt:Issuer"],
			audience: _config["Jwt:Audience"],
			claims: claims,
			expires: DateTime.UtcNow.AddHours(2),
			signingCredentials: creds
		);

		// 6️⃣ Return token
		return new LoginResponseDto
		{
			Token = new JwtSecurityTokenHandler().WriteToken(token)
		};
	}
}
