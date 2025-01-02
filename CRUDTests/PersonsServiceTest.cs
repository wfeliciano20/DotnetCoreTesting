using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.ENUMS;
using Services;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {

        private readonly IPeopleService _peopleService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        private IFixture _fixture;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            var countriesInitialData = new List<Country>() { };
            var peopleInitialData = new List<Person>() { };
            _fixture = new Fixture();

            DbContextMock<ApplicationDbContext> dbContextMock =
                new DbContextMock<ApplicationDbContext>(
                    new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            var dbContext = dbContextMock.Object;
            _countriesService = new CountriesService(dbContext);

            dbContextMock.CreateDbSetMock(x => x.Countries, countriesInitialData);
            dbContextMock.CreateDbSetMock(x => x.People, peopleInitialData);


            _peopleService = new PeopleService(dbContext, _countriesService);
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        // If Argument is null throw ArgumentNullException
        [Fact]
        public async Task AddPerson_nullArgument()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Assert

            Func<Task> action = async () =>
            await _peopleService.AddPerson(personAddRequest);

            await action.Should().ThrowAsync<ArgumentNullException>();

            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    // Act
            //    await _peopleService.AddPerson(personAddRequest);
            //});
        }

        // If name or email is null throw ArgumentException
        [Fact]
        public async Task AddPerson_nullArgumentPersonName()
        {
            // Arrange
            PersonAddRequest addRequest = _fixture.Build<PersonAddRequest>().With(p => p.PersonName, (string?)null).Create();
            //    new PersonAddRequest()
            //{
            //    PersonName = null,
            //};

            // Assert

            Func<Task> action = async () =>
            {
                // Act
                await _peopleService.AddPerson(addRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    // Act
            //    await _peopleService.AddPerson(addRequest);
            //});
        }

        // When valid argument Provided add it to the list
        [Fact]
        public async Task AddPerson_validArgument()
        {
            // Arrange
            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(p => p.Email, "person@email.com").Create();
            //    new PersonAddRequest()
            //{
            //    PersonName = "William",
            //    Email = "william@gmail.com",
            //    Address = "Some Address",
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    CountryID = Guid.NewGuid(),
            //    Gender = GenderOptions.MALE,
            //    ReceiveNewsLetter = true
            //};

            // Act
            PersonResponse actual = await _peopleService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = await _peopleService.GetAllPeople(null, null, null, null);

            // Assert
            actual.PersonID.Should().NotBe(Guid.Empty);
            //Assert.True(actual.PersonID != Guid.Empty);
            actual.Should().BeEquivalentTo(persons_list);
            //Assert.Contains(actual, persons_list);
        }

        #endregion

        #region GetPersonByID

        [Fact]
        public async Task GetPersonByID_NullIDProvided()
        {
            // Arrange
            Guid? personId = null;

            PersonResponse? actualResponse = await _peopleService.GetPersonByPersonID(personId);
            // Assert
            //Assert.Null(actualResponse);
            actualResponse.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByID_ProvidedAValidID()
        {
            // Arrange
            CountryResponse countryResponse = await CreateOneCountry();

            // Create the AddPersonRequest object
            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
                                                        .With(p => p.Email, "person@email.com").Create();
            //    new PersonAddRequest()
            //{
            //    PersonName = "Name...",
            //    Email = "test@test.com",
            //    Address = "address...",
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    CountryID = countryResponse.CountryId,
            //    Gender = GenderOptions.MALE,
            //    ReceiveNewsLetter = false
            //};

            // Add the person to the list of persons and save its new guid id
            PersonResponse expectedPersonResponse = await _peopleService.AddPerson(personAddRequest);


            //Act
            PersonResponse? actualPersonResponse = await _peopleService.GetPersonByPersonID(expectedPersonResponse.PersonID);

            // Assert
            //Assert.Equal(expectedPersonResponse, actualPersonResponse);
            actualPersonResponse.Should().BeEquivalentTo(expectedPersonResponse);
        }

        #endregion

        #region GetAllPeople

        [Fact]
        public async Task GetAllPeople_EmptyListIfNoPeopleAddedNoSortNoFilter()
        {
            // Act
            List<PersonResponse> personResponse = await _peopleService.GetAllPeople(null, null, null, null);

            // Assert
            //Assert.Empty(personResponse);

            personResponse.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllPeople_PeopleInListNoFilterNoSort()
        {

            List<PersonAddRequest> peopleToAdd = await CreatePeopleToAddListAscending();

            List<PersonResponse> expectedPeople = new List<PersonResponse>();
            _testOutputHelper.WriteLine("EXPECTED:");
            foreach (var person in peopleToAdd)
            {
                PersonResponse personResponse = await _peopleService.AddPerson(person);
                expectedPeople.Add(personResponse);
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            List<PersonResponse> actualPeople = await _peopleService.GetAllPeople(null, null, null, null);

            _testOutputHelper.WriteLine("ACTUAL:");

            foreach (var actualPerson in actualPeople)
            {
                _testOutputHelper.WriteLine(actualPerson.ToString());
            }

            foreach (var person in expectedPeople)
            {
                //Assert.Contains(person, actualPeople);
                actualPeople.Should().Contain(person);
            }
        }

        [Fact]
        public async Task GetAllPeople_SearchByProvidedSearchStringEmpty_ReturnsList()
        {

            List<PersonAddRequest> peopleToAdd = await CreatePeopleToAddListAscending();

            List<PersonResponse> expectedPeople = new List<PersonResponse>();

            _testOutputHelper.WriteLine("EXPECTED:");

            foreach (var person in peopleToAdd)
            {
                PersonResponse personResponse = await _peopleService.AddPerson(person);

                expectedPeople.Add(personResponse);
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            List<PersonResponse> actualPeople = await _peopleService.GetAllPeople("PersonName", "", null, null);

            _testOutputHelper.WriteLine("ACTUAL:");

            foreach (var actualPerson in actualPeople)
            {
                _testOutputHelper.WriteLine(actualPerson.ToString());
            }

            foreach (var person in expectedPeople)
            {
                //Assert.Contains(person, actualPeople);
                actualPeople.Should().Contain(person);
            }
        }


        [Fact]
        public async Task GetAllPeople__OrderByIsProvidedAndSortOptionsAscReturnsOrderedList()
        {

            List<PersonAddRequest> peopleToAdd = await CreatePeopleToAddListAscending();

            List<PersonResponse> expectedPeople = new List<PersonResponse>();

            _testOutputHelper.WriteLine("EXPECTED:");

            foreach (var person in peopleToAdd)
            {
                PersonResponse personResponse = await _peopleService.AddPerson(person);
                expectedPeople.Add(personResponse);
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            List<PersonResponse> actualPeople = await _peopleService.GetAllPeople(null, null, "PersonName", SortOptions.ASC);

            _testOutputHelper.WriteLine("ACTUAL:");

            if (actualPeople != null)
            {
                foreach (var actualPerson in actualPeople)
                {
                    _testOutputHelper.WriteLine(actualPerson.ToString());
                }
            }
            //Assert.Equal(expectedPeople, actualPeople);
            actualPeople.Should().BeEquivalentTo(expectedPeople);
        }


        [Fact]
        public async Task GetAllPeople__OrderByIsProvidedAndSortOptionsDescReturnsOrderedList()
        {
            List<PersonAddRequest> peopleToAdd = await CreatePeopleToAddListDescending();

            List<PersonResponse> expectedPeople = new List<PersonResponse>();

            _testOutputHelper.WriteLine("EXPECTED:");

            foreach (var person in peopleToAdd)
            {
                PersonResponse personResponse = await _peopleService.AddPerson(person);
                expectedPeople.Add(personResponse);
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            List<PersonResponse> actualPeople = await _peopleService.GetAllPeople(null, null, "PersonName", SortOptions.DESC);

            _testOutputHelper.WriteLine("ACTUAL:");

            if (actualPeople != null)
            {
                foreach (var actualPerson in actualPeople)
                {
                    _testOutputHelper.WriteLine(actualPerson.ToString());
                }
            }

            //Assert.Equal(expectedPeople, actualPeople);
            actualPeople.Should().BeEquivalentTo(expectedPeople);
            //Assert.NotNull(actualPeople?.First().Country);
            actualPeople?.First().Country.Should().NotBeNullOrEmpty();

        }

        #endregion

        #region UpdatePerson

        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            // Arrange
            PersonAddRequest personAdd = await CreateOnePerson();

            PersonResponse addedPerson = await _peopleService.AddPerson(personAdd);

            PersonUpdateRequest personUpdateRequest = addedPerson.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = null;

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _peopleService.UpdatePerson(personUpdateRequest);
            //});

            Func<Task> action = async () =>
            {
                await _peopleService.UpdatePerson(personUpdateRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_UpdateFullPersonDetailsWithWrongPersonID()
        {
            // Arrange
            PersonAddRequest personAdd = await CreateOnePerson();

            PersonResponse addedPerson = await _peopleService.AddPerson(personAdd);

            PersonUpdateRequest personUpdateRequest = addedPerson.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = "William";

            personUpdateRequest.Email = "william@example.com";

            personUpdateRequest.PersonID = Guid.NewGuid();

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _peopleService.UpdatePerson(personUpdateRequest);
            //});

            Func<Task> action = async () =>
            {
                await _peopleService.UpdatePerson(personUpdateRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public async Task UpdatePerson_UpdateFullPersonDetails()
        {
            // Arrange
            PersonAddRequest personAdd = await CreateOnePerson();

            PersonResponse addedPerson = await _peopleService.AddPerson(personAdd);

            PersonUpdateRequest personUpdateRequest = addedPerson.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = "William";

            personUpdateRequest.Email = "william@example.com";

            PersonResponse actualResponse = await _peopleService.UpdatePerson(personUpdateRequest);

            PersonResponse? expectedResponse = await _peopleService.GetPersonByPersonID(personUpdateRequest.PersonID);

            //Assert.Equal(expectedResponse, actualResponse);

            actualResponse.Should().BeEquivalentTo(expectedResponse);

        }



        #endregion

        #region DeletePerson

        [Fact]
        public async Task DeletePerson_PersonIdNUll()
        {
            Guid? id = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _peopleService.DeletePerson(id);
            });
        }

        [Fact]
        public async Task DeletePerson_PersonIdWrong()
        {
            Guid wrongId = Guid.NewGuid();

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _peopleService.DeletePerson(wrongId);
            });
        }

        [Fact]
        public async Task DeletePerson_PersonIdValid()
        {
            PersonAddRequest personAddRequest = await CreateOnePerson();

            PersonResponse personResponse = await _peopleService.AddPerson(personAddRequest);

            List<PersonResponse> initialAllPeople = await _peopleService.GetAllPeople(null, null, null, null);

            bool actualResponse = await _peopleService.DeletePerson(personResponse.PersonID);

            List<PersonResponse> finallAllPeople = await _peopleService.GetAllPeople(null, null, null, null);

            Assert.True(actualResponse);

            Assert.NotEqual(initialAllPeople, finallAllPeople);
        }

        #endregion

        #region helper Methods

        private async Task<PersonAddRequest> CreateOnePerson()
        {
            CountryResponse countryResponse = await CreateOneCountry();

            return new PersonAddRequest()
            {
                // Create the AddPersonRequest object
                PersonName = "John Jones",
                Email = "john@test.com",
                Address = "Niagara Falls Canada",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                CountryID = countryResponse.CountryId,
                Gender = GenderOptions.MALE,
                ReceiveNewsLetter = false
            };
        }

        private async Task<List<PersonAddRequest>> CreatePeopleToAddListAscending()
        {
            // Create the country request object 
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            //    new CountryAddRequest()
            //{
            //    CountryName = "Canada"
            //};

            // add the country and store the country Id
            CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personToAddOne = _fixture.Build<PersonAddRequest>().With(p => p.PersonName, "John").With(p => p.Email, "test1@test.com").With(p => p.CountryID, countryResponse.CountryId).Create();
            //    new PersonAddRequest()
            //{
            //    // Create the AddPersonRequest object
            //    PersonName = "John",
            //    Email = "test@test.com",
            //    Address = "address...",
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    CountryID = countryResponse.CountryId,
            //    Gender = GenderOptions.MALE,
            //    ReceiveNewsLetter = false
            //};

            PersonAddRequest personToAddTwo = _fixture.Build<PersonAddRequest>().With(p => p.PersonName, "Steve").With(p => p.Email, "test2@test.com").With(p => p.CountryID, countryResponse.CountryId).Create();
            //    new PersonAddRequest()
            //{
            //    // Create the AddPersonRequest object
            //    PersonName = "Steve",
            //    Email = "test2@test.com",
            //    Address = "address2...",
            //    DateOfBirth = DateTime.Parse("2000-02-02"),
            //    CountryID = countryResponse.CountryId,
            //    Gender = GenderOptions.MALE,
            //    ReceiveNewsLetter = true
            //};

            PersonAddRequest personToAddThree = _fixture.Build<PersonAddRequest>().With(p => p.PersonName, "Wilma").With(p => p.Email, "test3@test.com").With(p => p.CountryID, countryResponse.CountryId).Create();
            //    new PersonAddRequest()
            //{
            //    // Create the AddPersonRequest object
            //    PersonName = "Wilma",
            //    Email = "test3@test.com",
            //    Address = "address3...",
            //    DateOfBirth = DateTime.Parse("2000-03-03"),
            //    CountryID = countryResponse.CountryId,
            //    Gender = GenderOptions.FEMALE,
            //    ReceiveNewsLetter = true
            //};

            List<PersonAddRequest> peopleToAdd = new List<PersonAddRequest>()
            {
                personToAddOne,
                personToAddTwo,
                personToAddThree
            };
            return peopleToAdd;
        }

        private async Task<List<PersonAddRequest>> CreatePeopleToAddListDescending()
        {
            CountryResponse countryResponse = await CreateOneCountry();

            PersonAddRequest personToAddOne = _fixture.Build<PersonAddRequest>().With(p => p.PersonName, "John").With(p => p.Email, "test1@test.com").With(p => p.CountryID, countryResponse.CountryId).Create();
            //    new PersonAddRequest()
            //{
            //    // Create the AddPersonRequest object
            //    PersonName = "John",
            //    Email = "test@test.com",
            //    Address = "address...",
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    CountryID = countryResponse.CountryId,
            //    Gender = GenderOptions.MALE,
            //    ReceiveNewsLetter = false
            //};

            PersonAddRequest personToAddTwo = _fixture.Build<PersonAddRequest>().With(p => p.PersonName, "Many").With(p => p.Email, "test2@test.com").With(p => p.CountryID, countryResponse.CountryId).Create();
            //    new PersonAddRequest()
            //{
            //    // Create the AddPersonRequest object
            //    PersonName = "Many",
            //    Email = "test2@test.com",
            //    Address = "address2...",
            //    DateOfBirth = DateTime.Parse("2000-02-02"),
            //    CountryID = countryResponse.CountryId,
            //    Gender = GenderOptions.MALE,
            //    ReceiveNewsLetter = true
            //};

            PersonAddRequest personToAddThree = _fixture.Build<PersonAddRequest>().With(p => p.PersonName, "Wilma").With(p => p.Email, "test3@test.com").With(p => p.CountryID, countryResponse.CountryId).Create();
            //    new PersonAddRequest()
            //{
            //    // Create the AddPersonRequest object
            //    PersonName = "Wilma",
            //    Email = "test3@test.com",
            //    Address = "address3...",
            //    DateOfBirth = DateTime.Parse("2000-03-03"),
            //    CountryID = countryResponse.CountryId,
            //    Gender = GenderOptions.FEMALE,
            //    ReceiveNewsLetter = true
            //};

            List<PersonAddRequest> peopleToAdd = new List<PersonAddRequest>()
            {
                personToAddThree,
                personToAddTwo,
                personToAddOne
            };
            return peopleToAdd;
        }

        private async Task<CountryResponse> CreateOneCountry()
        {
            // Create the country request object 
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();

            //    new CountryAddRequest()
            //{
            //    CountryName = "Canada"
            //};

            // add the country and store the country Id
            CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);
            return countryResponse;
        }

        #endregion

    }
}