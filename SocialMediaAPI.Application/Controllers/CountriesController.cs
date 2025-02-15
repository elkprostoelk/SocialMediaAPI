using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.Common.DTO;
using SocialMediaAPI.Core.Interfaces;

namespace SocialMediaAPI.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CountryDto>), StatusCodes.Status200OK, "application/json")]
        public async Task<IActionResult> GetAllCountries()
        {
            return Ok(await _countryService.GetAllCountriesAsync());
        }
    }
}
