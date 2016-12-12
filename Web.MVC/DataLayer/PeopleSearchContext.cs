using System.Data.Entity;

namespace HealthCatalyst.DataLayer
{
    public class PeopleSearchContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<PersonInterest> PersonInterests { get; set; }

        static PeopleSearchContext()
        {
            Database.SetInitializer<PeopleSearchContext>(null);
        }

        public PeopleSearchContext() : base("Name=PeopleSearch")
        {
            Database.SetInitializer(new PeopleSearchInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new PersonConfiguration());
            modelBuilder.Configurations.Add(new PersonInterestConfiguration());
        }
    }
}