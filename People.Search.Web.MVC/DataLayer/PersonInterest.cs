namespace HealthCatalyst.DataLayer
{
    /// <summary>
    /// Stores one instance of a person's interst.
    /// </summary>
    public class PersonInterest
    {
        /// <summary>
        /// Identity Primary key.
        /// </summary>
        public int PersonInterestId { get; set; }

        /// <summary>
        /// Foreign-Key referring to person table's primary key.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Interest string. Max Length: 40
        /// </summary>
        public string Interest { get; set; }

        // 
        /// <summary>
        /// Person Foreign Key Entity Reference. (FK_Person_PersonInterest)
        /// </summary>
        public virtual Person Person { get; set; }

        public PersonInterest() { }

        public PersonInterest(string interest)
        {
            Interest = interest;
        }
    }

}