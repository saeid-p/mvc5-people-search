using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HealthCatalyst.DataLayer
{
    public class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration() : this("dbo") { }

        public PersonConfiguration(string schema)
        {
            ToTable("Person", schema);
            HasKey(x => x.PersonId);

            Property(x => x.PersonId).HasColumnName("PersonId").HasColumnType("int").IsRequired();
            Property(x => x.PersonId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.FirstName).HasColumnName("FirstName").HasColumnType("nvarchar").IsRequired().HasMaxLength(40);
            Property(x => x.LastName).HasColumnName("LastName").HasColumnType("nvarchar").IsRequired().HasMaxLength(40);
            Property(x => x.Address).HasColumnName("Address").HasColumnType("nvarchar").IsOptional().HasMaxLength(100);
            Property(x => x.BirthDate).HasColumnName("BirthDate").HasColumnType("date").IsOptional();
            Property(x => x.Picture).HasColumnName("Picture").HasColumnType("varbinary(max)").IsOptional();
        }
    }
}