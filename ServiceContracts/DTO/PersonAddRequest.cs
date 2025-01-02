using Entities;
using ServiceContracts.ENUMS;
using System.ComponentModel.DataAnnotations;

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
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Enter a valid date")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Select a gender")]
        public GenderOptions? Gender { get; set; }

        [Required(ErrorMessage = "Select a country")]
        public Guid? CountryID { get; set; }

        [Required(ErrorMessage = "Address is required")]
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