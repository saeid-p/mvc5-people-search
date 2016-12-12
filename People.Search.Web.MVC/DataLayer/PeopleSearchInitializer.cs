using System;
using System.Collections.Generic;
using System.Data.Entity;
using HealthCatalyst.Utilities;

namespace HealthCatalyst.DataLayer
{
    /// <summary>
    /// Entity Framework Mock Data Generator.
    /// </summary>
    public class PeopleSearchInitializer : CreateDatabaseIfNotExists<PeopleSearchContext>
    {
        protected override void Seed(PeopleSearchContext context)
        {
            var imageHelper = new ImageHelper();
            IList<Person> people = new List<Person>();

            people.Add(new Person
            {
                FirstName = "John",
                LastName = "Smith",
                BirthDate = DateTime.Now.AddYears(-70),
                Address = "9900 Abc St. XYZ Hwy. Apt 8567",
                Picture = imageHelper.BitmapToByteArray(Res.SampleImages.Sample1),
                PersonInterests = new List<PersonInterest>
                {
                    new PersonInterest("Reading"),
                    new PersonInterest("TV"),
                }
            });

            people.Add(new Person
            {
                FirstName = "John",
                LastName = "Roberts",
                BirthDate = DateTime.Now.AddYears(-40),
                Address = "8800 Jackson St. XYZ Hwy. Apt 3456",
                Picture = imageHelper.BitmapToByteArray(Res.SampleImages.Sample2),
                PersonInterests = new List<PersonInterest>
                {
                    new PersonInterest("Boxing"),
                    new PersonInterest("Sailing")
                }
            });

            people.Add(new Person
            {
                FirstName = "Jack",
                LastName = "Smith",
                Address = "7700 Holly Hall St. XYZ Hwy. Apt 6456",
                Picture = imageHelper.BitmapToByteArray(Res.SampleImages.Sample4),
                PersonInterests = new List<PersonInterest>
                {
                    new PersonInterest("Reading"),
                    new PersonInterest("Boxing"),
                }
            });

            people.Add(new Person
            {
                FirstName = "Sam",
                LastName = "Smith",
                BirthDate = DateTime.Now.AddYears(-50),
                PersonInterests = new List<PersonInterest>
                {
                    new PersonInterest("Sailing")
                }
            });

            people.Add(new Person
            {
                FirstName = "Mary",
                LastName = "Roberts",
                BirthDate = DateTime.Now.AddYears(-20),
                Address = "6600 Fannin St. XYZ Hwy. Apt 1235",
                Picture = imageHelper.BitmapToByteArray(Res.SampleImages.Sample3),
                PersonInterests = new List<PersonInterest>
                {
                    new PersonInterest("Shopping"),
                    new PersonInterest("Sailing")
                }
            });

            people.Add(new Person
            {
                FirstName = "Mary",
                LastName = "Johnson",
                BirthDate = DateTime.Now.AddYears(-28),
                Address = "2250 Main St. XYZ Hwy. Apt 3456",
                Picture = imageHelper.BitmapToByteArray(Res.SampleImages.Sample6)
            });

            context.People.AddRange(people);
            base.Seed(context);
        }
    }
}