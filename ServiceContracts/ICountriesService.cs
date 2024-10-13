using ServiceContracts.DTO;

namespace ServiceContracts;
/// <summary>
/// Represents business logic for manipulating
/// Country entity
/// </summary>
public interface ICountriesService
{
    /// <summary>
    /// Adds a country object to the list of countries
    /// </summary>
    /// <param name="countryAddRequest">Country Object to add</param>
    /// <returns>Returns the country after adding it (including the newly generated id)</returns>
    CountryResponse AddCountry(CountryAddRequest? countryAddRequest);


    /// <summary>
    /// Returns the List of Countries
    /// </summary>
    /// <returns>Returns List of CountryResponseDTO</returns>
    List<CountryResponse> GetAllCountries();

    /// <summary>
    /// Returns a country object based on the given id
    /// </summary>
    /// <param name="id">CountryId guid to search</param>
    /// <returns>Matching Country as CountryResponse</returns>
    CountryResponse? GetCountryByID(Guid? id);
}

