

using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.ENUMS;
using Services;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {

        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;

        private readonly ITestOutputHelper _testOutputHelper;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personsService = new PersonsService();
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        // If Argument is null throw ArgumentNullException
        [Fact]
        public void AddPerson_nullArgument()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _personsService.AddPerson(personAddRequest);
            });
        }

        // If name or email is null throw ArgumentException
        [Fact]
        public void AddPerson_nullArgumentPersonName()
        {
            // Arrange
            PersonAddRequest addRequest = new PersonAddRequest()
            {
                PersonName = null,
            };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _personsService.AddPerson(addRequest);
            });
        }

        // When valid argument Provided add it to the list
        [Fact]
        public void AddPerson_validArgument()
        {
            // Arrange
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "William",
                Email = "william@gmail.com",
                Address = "Some Address",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                CountryID = Guid.NewGuid(),
                Gender = GenderOptions.MALE,
                ReceiveNewsLetter = true
            };

            // Act
            PersonResponse actual = _personsService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = _personsService.GetAllPeople();

            // Assert
            Assert.True(actual.PersonID != Guid.Empty);
            Assert.Contains(actual, persons_list);
        }

        #endregion

        #region GetPersonByID

        [Fact]
        public void GetPersonByID_NullIDProvided()
        {
            // Arrange
            Guid? personId = null;

            PersonResponse? actualResponse = _personsService.GetPersonByPersonID(personId);
            // Assert
            Assert.Null(actualResponse);
        }

        [Fact]
        public void GetPersonByID_ProvidedAValidID()
        {
            // Arrange

            // Create the country request object 
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Canada"
            };

            // add the country and store the country Id
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            // Create the AddPersonRequest object
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Name...",
                Email = "test@test.com",
                Address = "address...",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                CountryID = countryResponse.CountryId,
                Gender = GenderOptions.MALE,
                ReceiveNewsLetter = false
            };

            // Add the person to the list of persons and save its new guid id
            PersonResponse expectedPersonResponse = _personsService.AddPerson(personAddRequest);

            //Act
            PersonResponse? actualPersonResponse = _personsService.GetPersonByPersonID(expectedPersonResponse.PersonID);

            // Assert
            Assert.Equal(expectedPersonResponse, actualPersonResponse);
        }

        #endregion

        #region GetAllPeople

        [Fact]
        public void GetAllPeople_EmptyListIfNoPeopleAdded()
        {
            // Act
            List<PersonResponse> personResponse = _personsService.GetAllPeople();

            // Assert
            Assert.Empty(personResponse);
        }

        [Fact]
        public void GetAllPeople_PeopleInList()
        {

            // Create the country request object 
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Canada"
            };

            // add the country and store the country Id
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personToAddOne = new PersonAddRequest()
            {
                // Create the AddPersonRequest object
                PersonName = "Name...",
                Email = "test@test.com",
                Address = "address...",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                CountryID = countryResponse.CountryId,
                Gender = GenderOptions.MALE,
                ReceiveNewsLetter = false
            };

            PersonAddRequest personToAddTwo = new PersonAddRequest()
            {
                // Create the AddPersonRequest object
                PersonName = "Name2...",
                Email = "test2@test.com",
                Address = "address2...",
                DateOfBirth = DateTime.Parse("2000-02-02"),
                CountryID = countryResponse.CountryId,
                Gender = GenderOptions.FEMALE,
                ReceiveNewsLetter = true
            };

            List<PersonAddRequest> peopleToAdd = new List<PersonAddRequest>()
            {
                personToAddOne,
                personToAddTwo
            };

            List<PersonResponse> expectedPeople = new List<PersonResponse>();
            _testOutputHelper.WriteLine("EXPECTED:");
            foreach (var person in peopleToAdd)
            {
                PersonResponse personResponse = _personsService.AddPerson(person);
                expectedPeople.Add(personResponse);
                _testOutputHelper.WriteLine(personResponse.ToString());
            }



            List<PersonResponse> actualPeople = _personsService.GetAllPeople();

            _testOutputHelper.WriteLine("ACTUAL:");

            foreach (var actualPerson in actualPeople)
            {
                _testOutputHelper.WriteLine(actualPerson.ToString());
            }

            foreach (var person in expectedPeople)
            {
                Assert.Contains(person, actualPeople);
            }
        }

        #endregion

    }
}