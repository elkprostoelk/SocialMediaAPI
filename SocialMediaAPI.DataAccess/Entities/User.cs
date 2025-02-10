using SocialMediaAPI.Common.Enums;

namespace SocialMediaAPI.DataAccess.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public required string Email { get; set; }

        public required string UserName { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? MiddleName { get; set; }

        public Gender Gender { get; set; }

        public DateOnly BirthDate { get; set; }

        public long CountryId { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }

        public string? Bio { get; set; }

        public string? AvatarImagePath { get; set; }

        public required string Salt { get; set; }

        public required string PasswordHash { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; } = default!;

        public Country Country { get; set; } = default!;
    }
}
