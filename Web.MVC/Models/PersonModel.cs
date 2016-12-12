using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCatalyst.Models
{
    /// <summary>
    /// Patient entity base model.
    /// </summary>
    public class PersonModel
    {
        public int PersonId { get; set; }

        [Required, MaxLength(40)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, MaxLength(40)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;

        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }
        public string BirthDateString => BirthDate?.ToShortDateString();
        public string Age => BirthDate.HasValue ? (DateTime.Now.Year - BirthDate.Value.Year).ToString() : null;

        [MaxLength(100)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Picture")]
        public byte[] Picture { get; set; }

        [Display(Name = "Interests")]
        public List<PersonInterestModel> Interests { get; set; } = new List<PersonInterestModel>();
    }
}