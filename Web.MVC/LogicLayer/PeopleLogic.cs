using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HealthCatalyst.DataLayer;
using HealthCatalyst.Models;

namespace HealthCatalyst.LogicLayer
{
    /// <summary>
    /// Encapsulates all required logic methods to interact with People entity.
    /// </summary>
    public class PeopleLogic : IPeopleLogic
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
        public async Task<List<PersonModel>> GetPeopleAsync(string query = null)
        {
            using (var db = new PeopleSearchContext())
            {
                IQueryable<Person> entity;
                if (query == null)
                    entity = db.People;
                else
                {
                    var loweredQuery = query.ToLower();
                    /* ToDo: The comparison with "Contains" (Like) is not a good idea here.
                     * It causes full table scan.
                     * Ideally I can establish a search system like ElasticSearch to implement this feature.
                     * Additionally, I can implement a full-text index to efficiently search these columns.
                     * The fields are lowered to ensure that if the collation of the SQL Server is CS, 
                     * it can evaluate all the queries provided by the user.
                    */
                    entity = db.People.Where(x =>
                        x.FirstName.ToLower().Contains(loweredQuery)
                        || x.LastName.ToLower().Contains(loweredQuery));
                }

                var results = entity.Select(x => new
                {
                    x.PersonId,
                    x.FirstName,
                    x.LastName,
                    x.BirthDate,
                    x.Address,
                    Interests = x.PersonInterests.Select(y => new
                    {
                        y.PersonInterestId,
                        y.PersonId,
                        y.Interest
                    })
                });

                var dataModel = await results.ToListAsync();
                return Mapper.Map<List<PersonModel>>(dataModel);
            }
        }

        /// <summary>
        /// Retrieves the picture of a person from the database.
        /// </summary>
        /// <param name="personId">Target person primary key.</param>
        /// <returns>Target person picture in byte[], or null if nothing found.</returns>
        public async Task<byte[]> GetPersonPictureAsync(int personId)
        {
            using (var db = new PeopleSearchContext())
            {
                var query = db.People
                    .Where(x => x.PersonId == personId)
                    .Select(x => new
                    {
                        x.Picture
                    });
                var dataModel = await query.FirstOrDefaultAsync();
                return Mapper.Map<byte[]>(dataModel.Picture);
            }
        }

        /// <summary>
        /// Adds a new person in the system.
        /// </summary>
        /// <param name="model">New person data model.</param>
        /// <returns>New database generated primary key of the person.</returns>
        public async Task<int> AddPersonAsync(PersonModel model)
        {
            var dataModel = Mapper.Map<Person>(model);
            dataModel.PersonInterests = Mapper.Map<List<PersonInterest>>(model.Interests);
            using (var db = new PeopleSearchContext())
            {
                db.People.Add(dataModel);
                await db.SaveChangesAsync();
            }
            return dataModel.PersonId;
        }

        /// <summary>
        /// Updates target person picture in the database.
        /// </summary>
        /// <param name="personId">Target person primary key.</param>
        /// <param name="bytes">The person picture in byte[].</param>
        public async Task EditPersonPicture(int personId, byte[] bytes)
        {
            var person = new Person
            {
                PersonId = personId,
                Picture = bytes
            };
            using (var db = new PeopleSearchContext())
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                db.People.Attach(person);
                db.Entry(person).Property(x => x.Picture).IsModified = true;
                await db.SaveChangesAsync();
            }
        }
    }
}