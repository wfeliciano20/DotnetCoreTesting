using ServiceContracts.DTO;
using ServiceContracts.ENUMS;

namespace ServiceContracts
{
    /// <summary>
    /// Represents Business Logic for manipulating Person entity
    /// </summary>
    public interface IPeopleService
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


        /// <summary>
        /// Updates a Person Object with the properties of the provided PersonUpdateRequest
        /// </summary>
        /// <param name="personUpdateRequest">An object with the values to be updated</param>
        /// <returns>A PersonResponse object with the validated properties</returns>
        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Deletes a person with the provided Guid id if the object is found.
        /// </summary>
        /// <param name="personID">The Guid to locate the person to delete</param>
        /// <returns>True if the deletion is successful False otherwise</returns>
        bool DeletePerson(Guid? personID);
    }
}