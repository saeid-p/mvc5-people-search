using System;
using System.Collections.Generic;

namespace HealthCatalyst.DataLayer
{
    /// <summary>
    /// Stores information about a person in the system.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Person table primary key.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// FirstName (length: 40)
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// LastName (length: 40)
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// BirthDate - Nullable
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Address (length: 100) - Nullable
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// Picture - Nullable
        /// </summary>
        public byte[] Picture { get; set; }

        /// <summary>
        /// Reverse navigation to the list of person's interests. (FK_Person_PersonInterest)
        /// </summary>
        public virtual ICollection<PersonInterest> PersonInterests { get; set; }

        public Person()
        {
            PersonInterests = new List<PersonInterest>();
        }
    }

}