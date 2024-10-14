using Entities;
using ServiceContracts.ENUMS;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Acts as a DTO for inserting a new Person
    /// </summary>
    public class PersonAddRequest
    {
        public string? PersonName { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public GenderOptions? Gender { get; set; }

        public Guid? CountryID { get; set; }

        public string? Address { get; set; }

        public bool ReceiveNewsLetter { get; set; }

        /// <summary>
        /// Converts PersonAddRequest object into Person Object
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryID = CountryID,
                Address = Address,
                ReceiveNewsLetter = ReceiveNewsLetter
            };
        }
    }

}