using Entities;
using Microsoft.EntityFrameworkCore;
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
        _countriesService = new CountriesService(new PeopleDbContext(new DbContextOptionsBuilder<PeopleDbContext>().Options));
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
        var allCountries = _countriesService.GetAllCountries();

        //Assert
        Assert.True(actual.CountryId != Guid.Empty);
        Assert.Contains(actual, allCountries);
    }

    #endregion

    #region GetAllCountries

    /*
        The list of countries should be empty by default
    */
    [Fact]
    public void GetAllCountries_EmptyList()
    {
        // Act
        List<CountryResponse> actual_country_response_list
                    = _countriesService.GetAllCountries();
        // Assert
        Assert.Empty(actual_country_response_list);
    }

    [Fact]
    public void GetAllCountries_WithTwoCountries()
    {
        // Arrange
        List<CountryAddRequest> country_request_list =
        new List<CountryAddRequest>()
        {
            new CountryAddRequest(){ CountryName = "United States"},
            new CountryAddRequest(){ CountryName = "United Kingdom"}
        };
        List<CountryResponse> add_country_response_list =
        new List<CountryResponse>();

        foreach (var country_request in country_request_list)
        {
            add_country_response_list.Add(_countriesService.AddCountry(country_request));
        }

        //Act
        List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();

        // Assert
        foreach (var expected_country_response in add_country_response_list)
        {
            Console.WriteLine(expected_country_response.CountryName);
            Assert.Contains(expected_country_response, actual_country_response_list);
        }
    }


    #endregion

    #region  GetCountryById

    [Fact]
    public void GetCountryByID_returnsNullWithNullID()
    {
        // Arrange
        Guid? countryID = null;
        // Act
        CountryResponse? result = _countriesService.GetCountryByID(countryID);
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetCountryByID_returnsCountryWhenValidIDIsGiven()
    {
        // Arrange
        CountryAddRequest countryAdd = new CountryAddRequest()
        {
            CountryName = "United States"
        };

        CountryResponse expectedCountry = _countriesService.AddCountry(countryAdd);

        // Act
        CountryResponse? actualResponse = _countriesService.GetCountryByID(expectedCountry.CountryId);

        Assert.Equal(expectedCountry, actualResponse);
    }

    [Fact]
    public void GetCountryByID_returnNullWhenGivenInvalidID()
    {
        // Arrange
        Guid invalidID = Guid.NewGuid();

        CountryAddRequest countryAdd = new CountryAddRequest()
        {
            CountryName = "United States"
        };

        CountryResponse expectedCountry = _countriesService.AddCountry(countryAdd);

        // Act
        CountryResponse? actual_response = _countriesService.GetCountryByID(invalidID);

        // Assert

        Assert.Null(actual_response);
    }



    #endregion
}
