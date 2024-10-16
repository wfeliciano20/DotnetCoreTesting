using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents Business Logic for manipulating Person entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Adds a new Person object into a Person List
        /// </summary>
        /// <param name="personAddRequest">The Person to be added to the list</param>
        /// <returns>The Person object with its newly created guid id</returns>
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);


        /// <summary>
        /// Returns the List of Person 
        /// </summary>
        /// <returns>The person list as a list of PersonResponse objects</returns>
        List<PersonResponse> GetAllPeople();

        /// <summary>
        /// Returns a Person that matches the provided id
        /// </summary>
        /// <param name="personID">PersonID to search</param>
        /// <returns>Matching Person</returns>
        PersonResponse? GetPersonByPersonID(Guid? personID);

        /// <summary>
        /// Returns a List of People that match the property of the searchBy with the value of the search string 
        /// </summary>
        /// <param name="searchBy">The Property you want to filtered with</param>
        /// <param name="searchString">The actual text value to filter with</param>
        /// <returns>The List of people that match the filter criteria</returns>
        List<PersonResponse>? GetFilteredPeople(string searchBy, string? searchString);

    }
}