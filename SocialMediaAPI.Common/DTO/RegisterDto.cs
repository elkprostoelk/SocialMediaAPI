using SocialMediaAPI.Common.Enums;

namespace SocialMediaAPI.Common.DTO
{
    public class RegisterDto
    {
        public string? UserName { get; set; }

        public required string Email { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateOnly BirthDate { get; set; }

        public long CountryId { get; set; }

        public required string Password { get; set; }
    }
}
