using System.ComponentModel.DataAnnotations;

namespace SwashBuckleExample.Models
{
    /// <summary>
    /// Represents a person.
    /// </summary>
    public class PersonModel
    {
        /// <summary>
        /// The person's full name.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// The person's age.
        /// </summary>
        [Range(0, 150)]
        public int Age { get; set; }
    }
}
