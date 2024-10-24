using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService(bool initialize = true)
    {
        _countries = new List<Country>();

        if (initialize)
        {
            _countries.AddRange(
                new List<Country>()
                {
                    new Country(){
                    CountryId= Guid.Parse("fb4fd053-27aa-4f82-b943-cb4808ce3918"),
                    CountryName = "USA"
                    },
                    new Country(){
                        CountryId= Guid.Parse("da0c5257-0b7b-4e69-892d-3cdfa6d08564"),
                        CountryName = "Canada"
                    },
                    new Country(){
                    CountryId= Guid.Parse("fee3ee7d-f715-4da2-8a0c-f70462cc26f7"),
                    CountryName = "UK"
                    },
                    new Country(){
                        CountryId= Guid.Parse("06e68e72-5619-4b5b-8a00-fccda2c20fd9"),
                        CountryName = "Australia"
                    }
                }
            );
        }
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
