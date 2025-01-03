using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.ENUMS;
using Services.Helpers;

namespace Services
{
    public class PeopleService : IPeopleService
    {

        private readonly ApplicationDbContext _dbContext;

        private readonly ICountriesService _countriesService;


        public PeopleService(ApplicationDbContext peopleDbContext, ICountriesService countriesService)
        {
            _dbContext = peopleDbContext;
            _countriesService = countriesService;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
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

            // Insert into db with stored procedure
            // _dbContext.sp_InsertPerson(newPerson);

            // ALTERNATIVE WAY

            // add person to list
            await _dbContext.People.AddAsync(newPerson);

            // Save Changes
            await _dbContext.SaveChangesAsync();

            return await ConvertPersonToPersonResponse(newPerson);
        }

        private async Task<PersonResponse> ConvertPersonToPersonResponse(Person newPerson)
        {
            PersonResponse newPersonResponse = newPerson.ToPersonResponse();

            if (newPersonResponse.CountryID != null)
            {
                var countryResponse = await _countriesService.GetCountryByID(newPersonResponse.CountryID);
                newPersonResponse.Country = countryResponse?.CountryName;
            }
            else
            {
                throw new ArgumentException("Invalid CountryId");
            }

            return newPersonResponse;
        }

        public async Task<List<PersonResponse>> GetAllPeople(string? searchBy, string? searchString, string? orderBy, SortOptions? sortOptions)
        {
            List<PersonResponse> allPeople = (await Task.WhenAll(_dbContext.People.ToList().Select(async p => await ConvertPersonToPersonResponse(p)))).ToList();

            if (string.IsNullOrEmpty(searchBy) && string.IsNullOrEmpty(searchString) && string.IsNullOrEmpty(orderBy) && sortOptions is null)
            {
                return allPeople;
            }

            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchString))
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

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if (personID is null)
            {
                return null;
            }

            Person? foundPerson = await _dbContext.People.FindAsync(personID);

            if (foundPerson is null)
            {
                return null;
            }

            return await ConvertPersonToPersonResponse(foundPerson);
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            // validate
            ValidationHelper.ModelValidation(personUpdateRequest);

            Person? foundPerson = await _dbContext.People.FirstOrDefaultAsync(p => p.PersonID == personUpdateRequest.PersonID);
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
            await _dbContext.SaveChangesAsync();

            // _dbContext.People.Select(p => p.PersonID == personUpdateRequest.PersonID ? foundPerson : p).ToList();

            return await ConvertPersonToPersonResponse(foundPerson);
        }

        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID is null)
            {
                throw new ArgumentNullException("PersonID must be provided");
            }

            Person? personFound = _dbContext.People.Find(personID);

            if (personFound is null)
            {
                throw new ArgumentException("Person Not Found");
            }

            _dbContext.People.Remove(personFound);

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}