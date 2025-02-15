using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SocialMediaAPI.Common.DTO;
using SocialMediaAPI.Core.Interfaces;
using SocialMediaAPI.DataAccess.Entities;
using SocialMediaAPI.DataAccess.Repositories;

namespace SocialMediaAPI.Core.Services
{
    public class CountryService : ICountryService
    {
        private const string CountriesCacheKey = "countries_cached";

        private readonly IMemoryCache _memoryCache;
        private readonly IRepository<Country, long> _countryRepository;
        private readonly IMapper _mapper;

        public CountryService(
            IMemoryCache memoryCache,
            IRepository<Country, long> countryRepository,
            IMapper mapper)
        {
            _memoryCache = memoryCache;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<List<CountryDto>> GetAllCountriesAsync()
        {
            return await _memoryCache.GetOrCreateAsync(CountriesCacheKey, async (x) =>
            {
                x.SetAbsoluteExpiration(TimeSpan.FromHours(12));
                var countries = await _countryRepository
                    .EntitySet
                    .AsNoTracking()
                    .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return countries;
            }) ?? [];
        }
    }
}
