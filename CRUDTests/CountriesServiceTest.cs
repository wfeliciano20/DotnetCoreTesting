using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit;

namespace CRUDTests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;

    public CountriesServiceTest()
    {
        _countriesService = new CountriesService();
    }

    #region AddCountry
    /*  
        Unit Test 1 Requirements
        When CountryAddRequest is null, it should
        Throw ArgumentNullException
    */
    [Fact]
    public void AddCountry_ArgumentIsNullThrowArgumentNullException()
    {
        // Arrange
        CountryAddRequest countryAddRequest = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() => _countriesService.AddCountry(countryAddRequest));

    }

    /*
        Unit test 2 Requirements
        When the country name is null it should
        throw ArgumentException
    */
    [Fact]
    public void AddCountry_AssertCountryNameNullThrowsArgumentException()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new CountryAddRequest()
        {
            CountryName = null
        };

        // Assert
        Assert.Throws<ArgumentException>(() => _countriesService.AddCountry(countryAddRequest));
    }

    /*  
        Unit Test 3 Requirements
        When the country name is duplicate, it should
        Throw ArgumentException
    */

    [Fact]
    public void AddCountry_WhenDuplicateNameThrowArgumentException()
    {
        CountryAddRequest countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Puerto Rico"
        };

        Assert.Throws<ArgumentException>(() =>
        {
            _countriesService.AddCountry(countryAddRequest);
            _countriesService.AddCountry(countryAddRequest);
        });
    }

    /*
        Unit test 4 Requirements
        When the country name is valid it should
        insert (add) the country to the existing
        list
    */
    [Fact]
    public void AddCountry_AddValidCountryAddsItToList()
    {
        //Arrange
        CountryAddRequest testCountry = new CountryAddRequest()
        {
            CountryName = "Puerto Rico"
        };

        Guid newCountryId = new Guid();

        Country newCountry = testCountry.ToCountry();
        newCountry.CountryId = newCountryId;

        CountryResponse expected = newCountry.ToCountryResponse();

        //Act
        CountryResponse actual = _countriesService.AddCountry(testCountry);

        //Assert
        Assert.True(actual.CountryId != Guid.Empty);
    }

    #endregion

    #region GetAllCountries


    #endregion

}
