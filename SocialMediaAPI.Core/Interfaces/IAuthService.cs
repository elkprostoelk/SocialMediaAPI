using SocialMediaAPI.Common.DTO;

namespace SocialMediaAPI.Core.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResultDto<string>> LoginAsync(LoginDto loginDto);
        Task<ServiceResultDto> RegisterAsync(RegisterDto registerDto);
    }
}
