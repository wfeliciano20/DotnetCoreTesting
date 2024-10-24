

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

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _peopleService = new PeopleService();
            _countriesService = new CountriesService(false);
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
                _peopleService.AddPerson(personAddRequest);
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
                _peopleService.AddPerson(addRequest);
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
            PersonResponse actual = _peopleService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = _peopleService.GetAllPeople(null, null, null, null);

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

            PersonResponse? actualResponse = _peopleService.GetPersonByPersonID(personId);
            // Assert
            Assert.Null(actualResponse);
        }

        [Fact]
        public void GetPersonByID_ProvidedAValidID()
        {
            // Arrange
            CountryResponse countryResponse = CreateOneCountry();

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
            PersonResponse expectedPersonResponse = _peopleService.AddPerson(personAddRequest);

            //Act
            PersonResponse? actualPersonResponse = _peopleService.GetPersonByPersonID(expectedPersonResponse.PersonID);

            // Assert
            Assert.Equal(expectedPersonResponse, actualPersonResponse);
        }

        #endregion

        #region GetAllPeople

        [Fact]
        public void GetAllPeople_EmptyListIfNoPeopleAddedNoSortNoFilter()
        {
            // Act
            List<PersonResponse> personResponse = _peopleService.GetAllPeople(null, null, null, null);

            // Assert
            Assert.Empty(personResponse);
        }

        [Fact]
        public void GetAllPeople_PeopleInListNoFilterNoSort()
        {

            List<PersonAddRequest> peopleToAdd = CreatePeopleToAddListAscending();

            List<PersonResponse> expectedPeople = new List<PersonResponse>();
            _testOutputHelper.WriteLine("EXPECTED:");
            foreach (var person in peopleToAdd)
            {
                PersonResponse personResponse = _peopleService.AddPerson(person);
                expectedPeople.Add(personResponse);
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            List<PersonResponse> actualPeople = _peopleService.GetAllPeople(null, null, null, null);

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

        [Fact]
        public void GetAllPeople_SearchByProvidedSearchStringEmpty_ReturnsList()
        {

            List<PersonAddRequest> peopleToAdd = CreatePeopleToAddListAscending();

            List<PersonResponse> expectedPeople = new List<PersonResponse>();

            _testOutputHelper.WriteLine("EXPECTED:");

            foreach (var person in peopleToAdd)
            {
                PersonResponse personResponse = _peopleService.AddPerson(person);
                expectedPeople.Add(personResponse);
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            List<PersonResponse> actualPeople = _peopleService.GetAllPeople("PersonName", "", null, null);

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


        [Fact]
        public void GetAllPeople__OrderByIsProvidedAndSortOptionsAscReturnsOrderedList()
        {

            List<PersonAddRequest> peopleToAdd = CreatePeopleToAddListAscending();

            List<PersonResponse> expectedPeople = new List<PersonResponse>();

            _testOutputHelper.WriteLine("EXPECTED:");

            foreach (var person in peopleToAdd)
            {
                PersonResponse personResponse = _peopleService.AddPerson(person);
                expectedPeople.Add(personResponse);
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            List<PersonResponse> actualPeople = _peopleService.GetAllPeople(null, null, "PersonName", SortOptions.ASC);

            _testOutputHelper.WriteLine("ACTUAL:");

            if (actualPeople != null)
            {
                foreach (var actualPerson in actualPeople)
                {
                    _testOutputHelper.WriteLine(actualPerson.ToString());
                }
            }
            Assert.Equal(expectedPeople, actualPeople);

        }


        [Fact]
        public void GetAllPeople__OrderByIsProvidedAndSortOptionsDescReturnsOrderedList()
        {
            List<PersonAddRequest> peopleToAdd = CreatePeopleToAddListDescending();

            List<PersonResponse> expectedPeople = new List<PersonResponse>();

            _testOutputHelper.WriteLine("EXPECTED:");

            foreach (var person in peopleToAdd)
            {
                PersonResponse personResponse = _peopleService.AddPerson(person);
                expectedPeople.Add(personResponse);
                _testOutputHelper.WriteLine(personResponse.ToString());
            }

            List<PersonResponse> actualPeople = _peopleService.GetAllPeople(null, null, "PersonName", SortOptions.DESC);

            _testOutputHelper.WriteLine("ACTUAL:");

            if (actualPeople != null)
            {
                foreach (var actualPerson in actualPeople)
                {
                    _testOutputHelper.WriteLine(actualPerson.ToString());
                }
            }
            Assert.Equal(expectedPeople, actualPeople);

        }

        #endregion

        #region UpdatePerson

        [Fact]
        public void UpdatePerson_PersonNameIsNull()
        {
            // Arrange
            PersonAddRequest personAdd = CreateOnePerson();

            PersonResponse addedPerson = _peopleService.AddPerson(personAdd);

            PersonUpdateRequest personUpdateRequest = addedPerson.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = null;

            Assert.Throws<ArgumentException>(() =>
            {
                _peopleService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_UpdateFullPersonDetailsWithWrongPersonID()
        {
            // Arrange
            PersonAddRequest personAdd = CreateOnePerson();

            PersonResponse addedPerson = _peopleService.AddPerson(personAdd);

            PersonUpdateRequest personUpdateRequest = addedPerson.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = "William";

            personUpdateRequest.Email = "william@example.com";

            personUpdateRequest.PersonID = Guid.NewGuid();

            Assert.Throws<ArgumentException>(() =>
            {
                _peopleService.UpdatePerson(personUpdateRequest);
            });
        }


        [Fact]
        public void UpdatePerson_UpdateFullPersonDetails()
        {
            // Arrange
            PersonAddRequest personAdd = CreateOnePerson();

            PersonResponse addedPerson = _peopleService.AddPerson(personAdd);

            PersonUpdateRequest personUpdateRequest = addedPerson.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = "William";

            personUpdateRequest.Email = "william@example.com";

            PersonResponse actualResponse = _peopleService.UpdatePerson(personUpdateRequest);

            PersonResponse? expectedResponse = _peopleService.GetPersonByPersonID(personUpdateRequest.PersonID);

            Assert.Equal(expectedResponse, actualResponse);

        }



        #endregion

        #region DeletePerson

        [Fact]
        public void DeletePerson_PersonIdNUll()
        {
            Guid? id = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _peopleService.DeletePerson(id);
            });
        }

        [Fact]
        public void DeletePerson_PersonIdWrong()
        {
            Guid wrongId = Guid.NewGuid();

            Assert.Throws<ArgumentException>(() =>
            {
                _peopleService.DeletePerson(wrongId);
            });
        }

        [Fact]
        public void DeletePerson_PersonIdValid()
        {
            PersonAddRequest personAddRequest = CreateOnePerson();

            PersonResponse personResponse = _peopleService.AddPerson(personAddRequest);

            List<PersonResponse> initialAllPeople = _peopleService.GetAllPeople(null, null, null, null);

            bool actualResponse = _peopleService.DeletePerson(personResponse.PersonID);

            List<PersonResponse> finallAllPeople = _peopleService.GetAllPeople(null, null, null, null);

            Assert.True(actualResponse);

            Assert.NotEqual(initialAllPeople, finallAllPeople);
        }

        #endregion

        #region helper Methods

        private PersonAddRequest CreateOnePerson()
        {
            CountryResponse countryResponse = CreateOneCountry();

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

        private List<PersonAddRequest> CreatePeopleToAddListAscending()
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
                PersonName = "John",
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
                PersonName = "Steve",
                Email = "test2@test.com",
                Address = "address2...",
                DateOfBirth = DateTime.Parse("2000-02-02"),
                CountryID = countryResponse.CountryId,
                Gender = GenderOptions.MALE,
                ReceiveNewsLetter = true
            };

            PersonAddRequest personToAddThree = new PersonAddRequest()
            {
                // Create the AddPersonRequest object
                PersonName = "Wilma",
                Email = "test3@test.com",
                Address = "address3...",
                DateOfBirth = DateTime.Parse("2000-03-03"),
                CountryID = countryResponse.CountryId,
                Gender = GenderOptions.FEMALE,
                ReceiveNewsLetter = true
            };

            List<PersonAddRequest> peopleToAdd = new List<PersonAddRequest>()
            {
                personToAddOne,
                personToAddTwo,
                personToAddThree
            };
            return peopleToAdd;
        }

        private List<PersonAddRequest> CreatePeopleToAddListDescending()
        {
            CountryResponse countryResponse = CreateOneCountry();

            PersonAddRequest personToAddOne = new PersonAddRequest()
            {
                // Create the AddPersonRequest object
                PersonName = "John",
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
                PersonName = "Many",
                Email = "test2@test.com",
                Address = "address2...",
                DateOfBirth = DateTime.Parse("2000-02-02"),
                CountryID = countryResponse.CountryId,
                Gender = GenderOptions.MALE,
                ReceiveNewsLetter = true
            };

            PersonAddRequest personToAddThree = new PersonAddRequest()
            {
                // Create the AddPersonRequest object
                PersonName = "Wilma",
                Email = "test3@test.com",
                Address = "address3...",
                DateOfBirth = DateTime.Parse("2000-03-03"),
                CountryID = countryResponse.CountryId,
                Gender = GenderOptions.FEMALE,
                ReceiveNewsLetter = true
            };

            List<PersonAddRequest> peopleToAdd = new List<PersonAddRequest>()
            {
                personToAddThree,
                personToAddTwo,
                personToAddOne
            };
            return peopleToAdd;
        }

        private CountryResponse CreateOneCountry()
        {
            // Create the country request object 
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Canada"
            };

            // add the country and store the country Id
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);
            return countryResponse;
        }

        #endregion

    }
}