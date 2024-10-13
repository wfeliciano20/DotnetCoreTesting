using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService()
    {
        _countries = new List<Country>();
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
        if (_countries.Find(c => c.CountryName == countryAddRequest.CountryName) != null)
        {
            throw new ArgumentException("A country with the same name already exists.", nameof(countryAddRequest.CountryName));
        }

        // convert dto to country
        Country country = countryAddRequest.ToCountry();

        // Generate new Guid
        country.CountryId = Guid.NewGuid();

        // Add to List
        _countries.Add(country);

        //return the DTO
        return country.ToCountryResponse();

    }

    public List<CountryResponse> GetAllCountries()
    {
        return _countries.Select(c => c.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryByID(Guid? id)
    {
        if (id is null)
        {
            return null;
        }

        Country? found_country = _countries.FirstOrDefault(c => c.CountryId == id);

        if (found_country is null)
        {
            return null;
        }

        return found_country.ToCountryResponse();
    }
}
