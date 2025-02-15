namespace SocialMediaAPI.Common.DTO
{
    public class CountryDto
    {
        public long Id { get; set; }

        public required string Name { get; set; }

        public required string PhoneCode { get; set; }
    }
}
