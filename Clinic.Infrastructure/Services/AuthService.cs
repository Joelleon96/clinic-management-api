using Clinic.Application.DTOs.Auth;
using Clinic.Application.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Infrastructure.Data;
using Clinic.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
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
	ClinicDbContext context,
	UserManager<ApplicationUser> userManager,
	ILogger<AuthService> logger)
	{
		_config = config;
		_context = context;
		_userManager = userManager;
		_logger = logger;
	}

	public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
	{
		
		var user = await _userManager.FindByEmailAsync(request.Email);

		if (user == null)
		{
			_logger.LogWarning("Login failed: User not found for email {Email}", request.Email);
			throw new UnauthorizedAccessException("Invalid credentials");
		}

		
		var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

		if (!isPasswordValid)
		{
			_logger.LogWarning("Login failed: Invalid password for email {Email}", request.Email);
			throw new UnauthorizedAccessException("Invalid credentials");
		}

		
		var roles = await _userManager.GetRolesAsync(user);

		
		var claims = new List<Claim>
	{
		new Claim(ClaimTypes.Email, user.Email!),
		new Claim(ClaimTypes.NameIdentifier, user.Id),
		new Claim("clinicId", user.ClinicEntityId.ToString())
	};

		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		
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

		return new LoginResponseDto
		{
			Token = new JwtSecurityTokenHandler().WriteToken(token)
		};
	}

	public async Task<string> RegisterAsync(RegisterRequestDto request)
	{
		using var transaction = await _context.Database.BeginTransactionAsync();

		try
		{
			var clinic = new ClinicEntity
			{
				Name = request.ClinicName,
				CreatedAt = DateTime.UtcNow
			};

			_context.Clinics.Add(clinic);
			await _context.SaveChangesAsync();

			var user = new ApplicationUser
			{
				UserName = request.Email,
				Email = request.Email,
				ClinicEntityId = clinic.Id
			};

			var result = await _userManager.CreateAsync(user, request.Password);

			if (!result.Succeeded)
			{
				throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
			}

			await _userManager.AddToRoleAsync(user, "Admin");

			await transaction.CommitAsync();

			return "User registered successfully";
		}
		catch
		{
			await transaction.RollbackAsync();
			throw;
		}
	}
}
