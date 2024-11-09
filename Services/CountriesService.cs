using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly PeopleDbContext _countriesDbContext;

    public CountriesService(PeopleDbContext dbContext)
    {
        _countriesDbContext = dbContext;
    }

    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
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
        if (_countriesDbContext.Countries.FirstOrDefault(c => c.CountryName == countryAddRequest.CountryName) != null)
        {
            throw new ArgumentException("A country with the same name already exists.", nameof(countryAddRequest.CountryName));
        }

        // convert dto to country
        Country country = countryAddRequest.ToCountry();

        // Generate new Guid
        country.CountryId = Guid.NewGuid();

        // Add to List
        _countriesDbContext.Countries.Add(country);

        // Save changes
        _countriesDbContext.SaveChanges();

        //return the DTO
        return country.ToCountryResponse();

    }

    public List<CountryResponse> GetAllCountries()
    {
        return _countriesDbContext.Countries.ToList().Select(c => c.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryByID(Guid? id)
    {
        if (id is null)
        {
            return null;
        }

        Country? found_country = _countriesDbContext.Countries.Find(id);

        if (found_country is null)
        {
            return null;
        }

        return found_country.ToCountryResponse();
    }
}
