using System.Collections.Generic;
using System.Threading.Tasks;
using HealthCatalyst.Models;

namespace HealthCatalyst.LogicLayer
{
    public interface IPeopleLogic
    {
        /// <summary>
        /// Retrieves a list of people stored in the system.
        /// </summary>
        /// <param name="query">
        /// Optional: A search term to search people with 
        /// the first name or the last name containing the search term.
        /// If no query provided (Null or empty), it returns all available records.
        /// </param>
        /// <returns>Returns the matched records based on the query provided.</returns>
        Task<List<PersonModel>> GetPeopleAsync(string query = null);

        /// <summary>
        /// Retrieves the picture of a person from the database.
        /// </summary>
        /// <param name="personId">Target person primary key.</param>
        /// <returns>Target person picture in byte[], or null if nothing found.</returns>
        Task<byte[]> GetPersonPictureAsync(int personId);

        /// <summary>
        /// Adds a new person in the system.
        /// </summary>
        /// <param name="model">New person data model.</param>
        /// <returns>New database generated primary key of the person.</returns>
        Task<int> AddPersonAsync(PersonModel model);

        /// <summary>
        /// Updates target person picture in the database.
        /// </summary>
        /// <param name="personId">Target person primary key.</param>
        /// <param name="bytes">The person picture in byte[].</param>
        Task EditPersonPicture(int personId, byte[] bytes);
    }
}