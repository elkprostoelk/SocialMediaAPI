using SocialMediaAPI.Common.DTO;

namespace SocialMediaAPI.Core.Interfaces
{
    public interface ICountryService
    {
        Task<List<CountryDto>> GetAllCountriesAsync();
    }
}
