

using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.ENUMS;
using Services;

namespace CRUDTests
{
    public class PersonsServiceTest
    {

        private readonly IPersonsService _personsService;

        public PersonsServiceTest()
        {
            _personsService = new PersonsService();
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
            List<PersonResponse> persons_list = _personsService.GetAllPersons();

            // Assert
            Assert.True(actual.PersonID != Guid.Empty);
            Assert.Contains(actual, persons_list);
        }

        #endregion
    }
}