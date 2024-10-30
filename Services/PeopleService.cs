using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.ENUMS;
using Services.Helpers;

namespace Services
{
    public class PeopleService : IPeopleService
    {

        private List<Person> _people;

        private readonly ICountriesService _countriesService;


        public PeopleService(bool initialize = true)
        {
            _people = new List<Person>();
            _countriesService = new CountriesService();
            if (initialize)
            {
                _people.AddRange(
                    new List<Person>()
                    {
                        new Person()
                        {
                            PersonID = Guid.Parse("a70565d2-9c96-4271-8115-2328346c4f0a"),
                            PersonName = "Toddie",
                            Email = "tstegers0@geocities.jp",
                            Address = "17 Portage Terrace",
                            DateOfBirth = DateTime.Parse("1997-05-22"),
                            CountryID = Guid.Parse("fb4fd053-27aa-4f82-b943-cb4808ce3918"),
                            Gender = "Male",
                            ReceiveNewsLetter = true
                        }, new Person()
                        {
                            PersonID = Guid.Parse("d8f0b56e-0624-49ad-b825-133de7568287"),
                            PersonName = "Eberhard",
                            Email = "eadamovicz1@yolasite.com",
                            Address = "88 Veith Street",
                            DateOfBirth = DateTime.Parse("1990-04-25"),
                            CountryID = Guid.Parse("da0c5257-0b7b-4e69-892d-3cdfa6d08564"),
                            Gender = "Male",
                            ReceiveNewsLetter = false
                        }, new Person()
                        {
                            PersonID = Guid.Parse("ca482969-0d0d-4ce6-ba4e-2e3ddcb6bb99"),
                            PersonName = "Kassey",
                            Email = "kspearett2@globo.com",
                            Address = "13177 Rockefeller Avenue",
                            DateOfBirth = DateTime.Parse("1991-12-01"),
                            CountryID = Guid.Parse("fee3ee7d-f715-4da2-8a0c-f70462cc26f7"),
                            Gender = "Female",
                            ReceiveNewsLetter = true
                        }, new Person()
                        {
                            PersonID = Guid.Parse("6226f1c9-dda4-47eb-a7f5-8e7c8ac8d027"),
                            PersonName = "Keriann",
                            Email = "kspringtorpe3@accuweather.com",
                            Address = "427 Armistice Plaza",
                            DateOfBirth = DateTime.Parse("1993-06-17"),
                            CountryID = Guid.Parse("06e68e72-5619-4b5b-8a00-fccda2c20fd9"),
                            Gender = "Female",
                            ReceiveNewsLetter = true
                        }, new Person()
                        {
                            PersonID = Guid.Parse("d2d19459-7860-4dfb-93f2-3efb657ac6ad"),
                            PersonName = "Fred",
                            Email = "fsedcole4@harvard.edu",
                            Address = "2 Forest Park",
                            DateOfBirth = DateTime.Parse("1990-12-11"),
                            CountryID = Guid.Parse("06e68e72-5619-4b5b-8a00-fccda2c20fd9"),
                            Gender = "Male",
                            ReceiveNewsLetter = false
                        }
                    }
                );
            }
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

            if (newPersonResponse.CountryID != null)
            {
                newPersonResponse.Country = _countriesService.GetCountryByID(newPersonResponse.CountryID)?.CountryName;
            }
            else
            {
                throw new ArgumentException("Invalid CountryId");
            }

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
                        .Where(p => p.DateOfBirth != null && p.DateOfBirth.Value.ToString("MM/dd/yyyy").Contains(searchString, StringComparison.Ordinal)).ToList(),
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
                    nameof(PersonResponse.DateOfBirth) => sortOptions.ToString() == "ASC" ? allPeople.OrderBy(p => p.DateOfBirth).ToList() : allPeople.OrderByDescending(p => p.DateOfBirth).ToList(),
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

            return ConvertPersonToPersonResponse(foundPerson);
        }

        public bool DeletePerson(Guid? personID)
        {
            if (personID is null)
            {
                throw new ArgumentNullException("PersonID must be provided");
            }

            Person? personFound = _people.Find(p => p.PersonID == personID);

            if (personFound is null)
            {
                throw new ArgumentException("Person Not Found");
            }

            return _people.Remove(personFound);


        }
    }
}