using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonsService
    {

        private readonly List<Person> _people;

        private readonly ICountriesService _countriesService;


        public PersonsService()
        {
            _people = new List<Person>();
            _countriesService = new CountriesService();
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest is null)
            {
                throw new ArgumentNullException("Argument can't be null");
            }

            // Model Validation throws ArgumentException if model is invalid
            ValidationHelper.ModelValidation(personAddRequest);

            // Convert to Person
            Person newPerson = personAddRequest.ToPerson();

            // simulate db creation and new guid assignment
            newPerson.PersonID = Guid.NewGuid();

            // add person to list
            _people.Add(newPerson);
            return ConvertPersonToPersonResponse(newPerson);
        }

        private PersonResponse ConvertPersonToPersonResponse(Person newPerson)
        {
            PersonResponse newPersonResponse = newPerson.ToPersonResponse();

            newPersonResponse.Country = _countriesService.GetCountryByID(newPersonResponse.CountryID)?.CountryName;

            return newPersonResponse;
        }

        public List<PersonResponse> GetAllPeople()
        {
            return _people.Select(p => ConvertPersonToPersonResponse(p)).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID is null)
            {
                return null;
            }

            Person? foundPerson = _people.FirstOrDefault(p => p.PersonID == personID);

            if (foundPerson is null)
            {
                return null;
            }

            return ConvertPersonToPersonResponse(foundPerson);
        }


    }
}