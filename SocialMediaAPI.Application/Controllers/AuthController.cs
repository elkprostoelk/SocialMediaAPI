using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.Common.DTO;
using SocialMediaAPI.Core.Interfaces;

namespace SocialMediaAPI.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IValidator<LoginDto> _loginDtoValidator;
        private readonly IValidator<RegisterDto> _registerDtoValidator;
        private readonly IAuthService _service;

        public AuthController(
            IValidator<LoginDto> loginDtoValidator,
            IAuthService service,
            IValidator<RegisterDto> registerDtoValidator)
        {
            _loginDtoValidator = loginDtoValidator;
            _service = service;
            _registerDtoValidator = registerDtoValidator;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(ServiceResultDto<string>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(ServiceResultDto<string>), StatusCodes.Status401Unauthorized, "application/json")]
        [ProducesResponseType(typeof(ServiceResultDto<string>), StatusCodes.Status400BadRequest, "application/json")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var validationResult = await _loginDtoValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var loginResult = await _service.LoginAsync(loginDto);
            
            return loginResult.Success
                ? Ok(loginResult)
                : Unauthorized(loginResult);
        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(ServiceResultDto), StatusCodes.Status201Created, "application/json")]
        [ProducesResponseType(typeof(ServiceResultDto), StatusCodes.Status400BadRequest, "application/json")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var validationResult = await _registerDtoValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var registerResult = await _service.RegisterAsync(registerDto);

            return registerResult.Success
                ? CreatedAtAction(nameof(Register), registerResult)
                : BadRequest(registerResult);
        }
    }
}
