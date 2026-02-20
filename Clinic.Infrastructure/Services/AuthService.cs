using Clinic.Application.DTOs.Auth;
using Clinic.Application.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Infrastructure.Data;
using Clinic.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
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
	private readonly UserManager<ApplicationUser> _userManager;


	public AuthService(
	IConfiguration config,
	UserManager<ApplicationUser> userManager,
	ILogger<AuthService> logger)
	{
		_config = config;
		_userManager = userManager;
		_logger = logger;
	}

	public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
	{
		// 1️⃣ Find user using Identity
		var user = await _userManager.FindByEmailAsync(request.Email);

		if (user == null)
		{
			_logger.LogWarning("Login failed: User not found for email {Email}", request.Email);
			throw new UnauthorizedAccessException("Invalid credentials");
		}

		// 2️⃣ Verify password using Identity
		var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

		if (!isPasswordValid)
		{
			_logger.LogWarning("Login failed: Invalid password for email {Email}", request.Email);
			throw new UnauthorizedAccessException("Invalid credentials");
		}

		// 3️⃣ Get roles
		var roles = await _userManager.GetRolesAsync(user);

		// 4️⃣ Create claims
		var claims = new List<Claim>
	{
		new Claim(ClaimTypes.Email, user.Email!),
		new Claim(ClaimTypes.NameIdentifier, user.Id),
		new Claim("ClinicEntityId", user.ClinicEntityId.ToString())
	};

		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		// 5️⃣ Generate signing key
		var key = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
		);

		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		// 6️⃣ Create token
		var token = new JwtSecurityToken(
			issuer: _config["Jwt:Issuer"],
			audience: _config["Jwt:Audience"],
			claims: claims,
			expires: DateTime.UtcNow.AddHours(2),
			signingCredentials: creds
		);

		return new LoginResponseDto
		{
			Token = new JwtSecurityTokenHandler().WriteToken(token)
		};
	}
}
