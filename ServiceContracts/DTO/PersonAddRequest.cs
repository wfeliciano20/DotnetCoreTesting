using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.ENUMS;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Acts as a DTO for inserting a new Person
    /// </summary>
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "PersonName can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be null")]
        [EmailAddress(ErrorMessage = "Email has to be a valid email")]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public GenderOptions? Gender { get; set; }

        public Guid? CountryID { get; set; }

        public string? Address { get; set; }

        [Required(ErrorMessage = "ReceiveNewsLetter is required.")]
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