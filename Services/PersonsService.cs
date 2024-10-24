using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.ENUMS;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonsService
    {

        private List<Person> _people;

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

        public List<PersonResponse> GetAllPeople(string? searchBy, string? searchString, string? orderBy, SortOptions? sortOptions)
        {
            List<PersonResponse> allPeople = _people.Select(p => ConvertPersonToPersonResponse(p)).ToList();

            if (string.IsNullOrEmpty(searchBy) && string.IsNullOrEmpty(searchString) && string.IsNullOrEmpty(orderBy) && sortOptions is null)
            {
                return allPeople;
            }

            if (!string.IsNullOrEmpty(searchBy))
            {
                // Filter by
                allPeople = searchBy switch
                {
                    nameof(PersonResponse.PersonName) => allPeople
                        .Where(p => !string.IsNullOrEmpty(p.PersonName) && p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    nameof(PersonResponse.Email) => allPeople
                        .Where(p => !string.IsNullOrEmpty(p.Email) && p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    nameof(PersonResponse.DateOfBirth) => allPeople
                        .Where(p => p.DateOfBirth != null && p.DateOfBirth.Value.ToString("MM/dd/yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    nameof(PersonResponse.Gender) => allPeople
                        .Where(p => !string.IsNullOrEmpty(p.Gender) && p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    nameof(PersonResponse.Country) => allPeople
                        .Where(p => !string.IsNullOrEmpty(p.Country) && p.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    nameof(PersonResponse.Address) => allPeople
                        .Where(p => !string.IsNullOrEmpty(p.Address) && p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    _ => allPeople
                };
            }

            // OrderBy
            if (orderBy is not null && sortOptions is not null)
            {
                allPeople = orderBy switch
                {
                    nameof(PersonResponse.PersonID) => sortOptions.ToString() == "ASC" ? allPeople.OrderBy(p => p.PersonID).ToList() : allPeople.OrderByDescending(p => p.PersonID).ToList(),
                    nameof(PersonResponse.PersonName) => sortOptions.ToString() == "ASC" ? allPeople.OrderBy(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList() : allPeople.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                    nameof(PersonResponse.Email) => sortOptions.ToString() == "ASC" ? allPeople.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList() : allPeople.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                    nameof(PersonResponse.Address) => sortOptions.ToString() == "ASC" ? allPeople.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList() : allPeople.OrderByDescending(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                    nameof(PersonResponse.Age) => sortOptions.ToString() == "ASC" ? allPeople.OrderBy(p => p.Age).ToList() : allPeople.OrderByDescending(p => p.Age).ToList(),
                    nameof(PersonResponse.Country) => sortOptions.ToString() == "ASC" ? allPeople.OrderBy(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList() : allPeople.OrderByDescending(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                    nameof(PersonResponse.CountryID) => sortOptions.ToString() == "ASC" ? allPeople.OrderBy(p => p.CountryID).ToList() : allPeople.OrderByDescending(p => p.CountryID).ToList(),
                    nameof(PersonResponse.DateOfBirth) => sortOptions.ToString() == "ASC" ? allPeople.OrderBy(p => p.DateOfBirth.ToString(), StringComparer.OrdinalIgnoreCase).ToList() : allPeople.OrderByDescending(p => p.DateOfBirth.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                    nameof(PersonResponse.Gender) => sortOptions.ToString() == "ASC" ? allPeople.OrderBy(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList() : allPeople.OrderByDescending(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                    nameof(PersonResponse.ReceiveNewsLetter) => sortOptions.ToString() == "ASC" ? allPeople.OrderBy(p => p.ReceiveNewsLetter).ToList() : allPeople.OrderByDescending(p => p.ReceiveNewsLetter).ToList(),
                    _ => allPeople
                };
            }

            return allPeople;

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

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            // validate
            ValidationHelper.ModelValidation(personUpdateRequest);

            Person? foundPerson = _people.FirstOrDefault(p => p.PersonID == personUpdateRequest.PersonID);
            if (foundPerson is null)
            {
                throw new ArgumentException("Person not found");
            }

            foundPerson.PersonName = personUpdateRequest.PersonName;
            foundPerson.Email = personUpdateRequest.Email;
            foundPerson.Address = personUpdateRequest.Address;
            foundPerson.CountryID = personUpdateRequest.CountryID;
            foundPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            foundPerson.Gender = personUpdateRequest.Gender.ToString();
            foundPerson.ReceiveNewsLetter = personUpdateRequest.ReceiveNewsLetter;

            // Save the changes
            _people = _people.Select(p => p.PersonID == personUpdateRequest.PersonID ? foundPerson : p).ToList();

            return foundPerson.ToPersonResponse();
        }
    }
}