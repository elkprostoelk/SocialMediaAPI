namespace SocialMediaAPI.DataAccess.Entities
{
    public class Country
    {
        public long Id { get; set; }

        public required string Name { get; set; }

        public required string PhoneCode { get; set; }

        public List<User> Users { get; set; } = [];
    }
}
