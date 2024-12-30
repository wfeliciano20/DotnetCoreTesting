using Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly ApplicationDbContext _countriesDbContext;

    public CountriesService(ApplicationDbContext dbContext)
    {
        _countriesDbContext = dbContext;
    }

    public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
    {

        // Chack that argument is not null
        if (countryAddRequest is null)
        {
            throw new ArgumentNullException("Argument can't be null");
        }

        //Check name is not null
        if (countryAddRequest.CountryName is null)
        {
            throw new ArgumentException("The country name can't be null");
        }

        // Check for duplicates
        if (await _countriesDbContext.Countries.FirstOrDefaultAsync(c => c.CountryName == countryAddRequest.CountryName) != null)
        {
            throw new ArgumentException("A country with the same name already exists.", nameof(countryAddRequest.CountryName));
        }

        // convert dto to country
        Country country = countryAddRequest.ToCountry();

        // Generate new Guid
        country.CountryId = Guid.NewGuid();

        // Add to List
        await _countriesDbContext.Countries.AddAsync(country);

        // Save changes
        await _countriesDbContext.SaveChangesAsync();

        //return the DTO
        return country.ToCountryResponse();

    }

    public async Task<List<CountryResponse>> GetAllCountries()
    {
        var countries = await _countriesDbContext.Countries.ToListAsync();
        return countries.Select(c => c.ToCountryResponse()).ToList();
    }

    public async Task<CountryResponse?> GetCountryByID(Guid? id)
    {
        if (id is null)
        {
            return null;
        }

        Country? found_country = await _countriesDbContext.Countries.FindAsync(id);

        if (found_country is null)
        {
            return null;
        }

        return found_country.ToCountryResponse();
    }
}
