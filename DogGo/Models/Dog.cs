using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Dog
    {
        public int? Id { get; set; }

        [Required]
        [DisplayName("Name")]
        public string? Name { get; set; }

        [Required]
        public int? OwnerId { get; set; }
        [Required]
        [DisplayName("Breed")]
        [StringLength(55, MinimumLength = (5))]
        public string? Breed { get; set; }

        [DisplayName("Notes")]
        [StringLength(256)]
        public string? Notes { get; set; }

        [DisplayName("Image Url")]
        [StringLength(256)]

        public string? ImageUrl { get; set; }
    }
}
