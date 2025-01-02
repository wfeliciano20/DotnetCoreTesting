using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;

    public CountriesServiceTest()
    {
        var countriesInitialData = new List<Country>() { };
        DbContextMock<ApplicationDbContext> dbContextMock =
            new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options);
        var dbContext = dbContextMock.Object;
        _countriesService = new CountriesService(dbContext);

        dbContextMock.CreateDbSetMock(x => x.Countries, countriesInitialData);
    }

    #region AddCountry
    /*  
        Unit Test 1 Requirements
        When CountryAddRequest is null, it should
        Throw ArgumentNullException
    */
    [Fact]
    public async Task AddCountry_ArgumentIsNullThrowArgumentNullException()
    {
        // Arrange
        CountryAddRequest countryAddRequest = null;

        // Assert
        //await Assert.ThrowsAsync<ArgumentNullException>(async () => await _countriesService.AddCountry(countryAddRequest));
        Func<Task> action = async () => await _countriesService.AddCountry(countryAddRequest);
        await action.Should().ThrowAsync<ArgumentNullException>();

    }

    /*
        Unit test 2 Requirements
        When the country name is null it should
        throw ArgumentException
    */
    [Fact]
    public async Task AddCountry_AssertCountryNameNullThrowsArgumentException()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new CountryAddRequest()
        {
            CountryName = null
        };

        // Assert
        //await Assert.ThrowsAsync<ArgumentException>(async () => await _countriesService.AddCountry(countryAddRequest));

        Func<Task> action = async () => await _countriesService.AddCountry(countryAddRequest);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    /*  
        Unit Test 3 Requirements
        When the country name is duplicate, it should
        Throw ArgumentException
    */

    [Fact]
    public async Task AddCountry_WhenDuplicateNameThrowArgumentException()
    {
        CountryAddRequest countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Puerto Rico"
        };

        //await Assert.ThrowsAsync<ArgumentException>(async () =>
        //{
        //    await _countriesService.AddCountry(countryAddRequest);
        //    await _countriesService.AddCountry(countryAddRequest);
        //});

        Func<Task> action = async () =>
        {
            await _countriesService.AddCountry(countryAddRequest);
            await _countriesService.AddCountry(countryAddRequest);
        };

        await action.Should().ThrowAsync<ArgumentException>();

    }

    /*
        Unit test 4 Requirements
        When the country name is valid it should
        insert (add) the country to the existing
        list
    */
    [Fact]
    public async Task AddCountry_AddValidCountryAddsItToList()
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
        CountryResponse actual = await _countriesService.AddCountry(testCountry);
        var allCountries = await _countriesService.GetAllCountries();

        //Assert
        //Assert.True(actual.CountryId != Guid.Empty);

        actual.CountryId.Should().NotBe(Guid.Empty);
        //Assert.Contains(actual, allCountries);
        allCountries.Should().ContainEquivalentOf(expected);
    }

    #endregion

    #region GetAllCountries

    /*
        The list of countries should be empty by default
    */
    [Fact]
    public async Task GetAllCountries_EmptyList()
    {
        // Act
        List<CountryResponse> actual_country_response_list
                    = await _countriesService.GetAllCountries();
        // Assert
        //Assert.Empty(actual_country_response_list);
        actual_country_response_list.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllCountries_WithTwoCountries()
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
            add_country_response_list.Add(await _countriesService.AddCountry(country_request));
        }

        //Act
        List<CountryResponse> actual_country_response_list = await _countriesService.GetAllCountries();

        // Assert
        foreach (var expected_country_response in add_country_response_list)
        {
            //Assert.Contains(expected_country_response, actual_country_response_list);
            actual_country_response_list.Should().ContainEquivalentOf(expected_country_response);
        }
    }


    #endregion

    #region  GetCountryById

    [Fact]
    public async Task GetCountryByID_returnsNullWithNullID()
    {
        // Arrange
        Guid? countryID = null;
        // Act
        CountryResponse? result = await _countriesService.GetCountryByID(countryID);
        // Assert
        // Assert.Null(result);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCountryByID_returnsCountryWhenValidIDIsGiven()
    {
        // Arrange
        CountryAddRequest countryAdd = new CountryAddRequest()
        {
            CountryName = "United States"
        };

        CountryResponse expectedCountry = await _countriesService.AddCountry(countryAdd);


        // Act
        CountryResponse? actualResponse = await _countriesService.GetCountryByID(expectedCountry.CountryId);

        // Assert.Equal(expectedCountry, actualResponse);
        actualResponse.Should().Equals(expectedCountry);
    }

    [Fact]
    public async Task GetCountryByID_returnNullWhenGivenInvalidID()
    {
        // Arrange
        Guid invalidID = Guid.NewGuid();

        CountryAddRequest countryAdd = new CountryAddRequest()
        {
            CountryName = "United States"
        };

        CountryResponse expectedCountry = await _countriesService.AddCountry(countryAdd);

        // Act
        CountryResponse? actual_response = await _countriesService.GetCountryByID(invalidID);

        // Assert

        //Assert.Null(actual_response);
        actual_response.Should().BeNull();
    }



    #endregion
}
