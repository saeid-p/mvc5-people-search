using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HealthCatalyst.DataLayer
{
    public class PersonInterestConfiguration : EntityTypeConfiguration<PersonInterest>
    {
        public PersonInterestConfiguration() : this("dbo") { }

        public PersonInterestConfiguration(string schema)
        {
            ToTable("PersonInterest", schema);
            HasKey(x => x.PersonInterestId);

            Property(x => x.PersonInterestId).HasColumnName("PersonInterestId").HasColumnType("int").IsRequired();
            Property(x => x.PersonInterestId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PersonId).HasColumnName("PersonId").HasColumnType("int").IsRequired();
            Property(x => x.Interest).HasColumnName("Interest").HasColumnType("nvarchar").IsRequired().HasMaxLength(40);

            // Foreign Key (FK_Person_PersonInterest)
            HasRequired(a => a.Person).WithMany(b => b.PersonInterests).HasForeignKey(c => c.PersonId);
        }
    }
}