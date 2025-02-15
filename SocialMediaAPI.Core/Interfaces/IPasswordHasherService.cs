namespace SocialMediaAPI.Core.Interfaces
{
    public interface IPasswordHasherService
    {
        (string salt, string hash) HashPassword(string password, string? oldSalt = null);
    }
}
