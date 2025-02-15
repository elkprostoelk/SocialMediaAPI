using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMediaAPI.Common.DTO;
using SocialMediaAPI.Common.Exceptions;
using SocialMediaAPI.Core.Interfaces;
using SocialMediaAPI.DataAccess.Entities;
using SocialMediaAPI.DataAccess.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMediaAPI.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IRepository<Role, int> _roleRepository;

        public AuthService(
            IRepository<User, Guid> userRepository,
            IPasswordHasherService passwordHasherService,
            IConfiguration configuration,
            IMapper mapper,
            IRepository<Role, int> roleRepository)
        {
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
            _configuration = configuration;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        public async Task<ServiceResultDto<string>> LoginAsync(LoginDto loginDto)
        {
            var result = new ServiceResultDto<string>();

            var user = await _userRepository
                .EntitySet
                .Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == loginDto.Login || u.Email == loginDto.Login);
            if (user is null)
            {
                result.Success = false;
                result.Errors.Add("User does not exist.");
                return result;
            }

            var (_, hashedPassword) = _passwordHasherService.HashPassword(loginDto.Password, user.Salt);

            result.Success = hashedPassword == user.PasswordHash;
            result.Errors = result.Success ? [] : ["Invalid password."];

            if (result.Success)
            {
                result.Result = GenerateAuthToken(user);
            }

            return result;
        }

        public async Task<ServiceResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var result = new ServiceResultDto();

            var userExists = await _userRepository
            .EntitySet
            .AnyAsync(u => u.Email == registerDto.Email
                || u.UserName == registerDto.UserName);

            if (userExists)
            {
                result.Success = false;
                result.Errors.Add("User with such email address or/and username already exists.");
                return result;
            }

            var user = _mapper.Map<User>(registerDto);
            var userRole = await _roleRepository
                .EntitySet
                .AsNoTracking()
                .SingleOrDefaultAsync(r => r.Name == "User") ?? throw new RoleNotFoundException();

            user.RoleId = userRole.Id;
            (user.Salt, user.PasswordHash) = _passwordHasherService.HashPassword(registerDto.Password);

            var created = await _userRepository.InsertAsync(user);
            result.Success = created;
            result.Errors = created ? [] : ["Failed to create a user account."];

            return result;
        }

        private string GenerateAuthToken(User user)
        {
            var jwtConfig = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]!);
            var secret = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Role, user.Role.Name)
            };

            var jwtSettings = _configuration.GetSection("JwtConfig");
            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["ValidIssuer"],
                audience: jwtSettings["ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddHours(Convert.ToDouble(jwtSettings["ExpiresIn"])),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
