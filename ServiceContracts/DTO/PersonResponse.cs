
using Entities;
using ServiceContracts.DTO;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO for most methods of PersonsService
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public Guid? CountryID { get; set; }

        public string? Country { get; set; }

        public string? Address { get; set; }

        public bool ReceiveNewsLetter { get; set; }

        public int? Age { get; set; }

        /// <summary>
        /// Compares to PersonResponse object's properties to determine equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True or False if all the properties are Equal</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(PersonResponse))
                return false;

            PersonResponse objectToCompare = (PersonResponse)obj;
            return this.PersonID == objectToCompare.PersonID && this.PersonName == objectToCompare.PersonName
                && this.Email == objectToCompare.Email && this.DateOfBirth == objectToCompare.DateOfBirth
                && this.CountryID == objectToCompare.CountryID && this.Country == objectToCompare.Country
                && this.Address == objectToCompare.Address && this.Gender == objectToCompare.Gender
                && this.Age == objectToCompare.Age && this.ReceiveNewsLetter == objectToCompare.ReceiveNewsLetter;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

/// <summary>
/// Static class for extension methods related to Person
/// </summary>
public static class PersonExtensions
{
    /// <summary>
    /// Convert A Person object into a Person Response object
    /// </summary>
    /// <param name="person">The Person object to be converted</param>
    /// <returns>The PersonResponse object after the conversion is done</returns>
    public static PersonResponse ToPersonResponse(this Person person)
    {
        return new PersonResponse()
        {
            PersonID = person.PersonID,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            Gender = person.Gender,
            CountryID = person.CountryID,
            Country = null,
            Address = person.Address,
            ReceiveNewsLetter = person.ReceiveNewsLetter,
            Age = (person.DateOfBirth is not null) ? (int)(Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25)) : null
        };
    }
}