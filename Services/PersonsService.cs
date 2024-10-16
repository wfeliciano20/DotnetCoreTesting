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

        public List<PersonResponse>? GetFilteredPeople(string searchBy, string? searchString)
        {
            List<PersonResponse> matchingPeople = _people.Select(p => p.ToPersonResponse()).ToList();

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchingPeople;
            }


            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPeople = matchingPeople
                    .Where(p => (!string.IsNullOrEmpty(p.PersonName) ? p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.Email):
                    matchingPeople = matchingPeople
                    .Where(p => (!string.IsNullOrEmpty(p.Email) ? p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.DateOfBirth):
                    matchingPeople = matchingPeople
                    .Where(p => (p.DateOfBirth != null) ? p.DateOfBirth.Value.ToString("mm/dd/yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    matchingPeople = matchingPeople
                    .Where(p => (!string.IsNullOrEmpty(p.Gender) ? p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Country):
                    matchingPeople = matchingPeople
                    .Where(p => (!string.IsNullOrEmpty(p.Country) ? p.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.Address):
                    matchingPeople = matchingPeople
                    .Where(p => (!string.IsNullOrEmpty(p.Address) ? p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                default:
                    // return all the matching people so do nothing here
                    break;

            }

            return matchingPeople;
        }
    }
}