using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Owner
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hmm ..Do you even name, bro?")]
        [MaxLength(35)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Seriously, where do you even live?")]
        [StringLength(55, MinimumLength =(5))]
        public string Address { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Phone]
        [DisplayName("Phone Number")]
        public string Phone { get; set; }

        [Required]
        [DisplayName("Neighborhood")]
        public int NeighborhoodId { get; set;}

        public Neighborhood Neighborhood { get; set; }

    }
}
