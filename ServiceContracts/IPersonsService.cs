using ServiceContracts.DTO;
using ServiceContracts.ENUMS;

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
        /// Returns the list of people if no argument is provided, if arguments are provided
        /// returns the filtered and ordered list.
        /// </summary>
        /// <param name="searchBy">The property on which you want to perform the search</param>
        /// <param name="searchString">The query string you are searching with</param>
        /// <param name="orderBy">the property on whihc you want to perform the sorting</param>
        /// <param name="sortOptions">The order of the sorting</param>
        /// <returns>The list of people, which can be sorted or filtered</returns>
        List<PersonResponse> GetAllPeople(string? searchBy, string? searchString, string? orderBy, SortOptions? sortOptions);

        /// <summary>
        /// Returns a Person that matches the provided id
        /// </summary>
        /// <param name="personID">PersonID to search</param>
        /// <returns>Matching Person</returns>
        PersonResponse? GetPersonByPersonID(Guid? personID);

        // /// <summary>
        // /// Returns a List of People that match the property of the searchBy with the value of the search string 
        // /// </summary>
        // /// <param name="searchBy">The Property you want to filtered with</param>
        // /// <param name="searchString">The actual text value to filter with</param>
        // /// <returns>The List of people that match the filter criteria</returns>
        // List<PersonResponse>? GetFilteredPeople(string searchBy, string? searchString);

        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);
    }
}