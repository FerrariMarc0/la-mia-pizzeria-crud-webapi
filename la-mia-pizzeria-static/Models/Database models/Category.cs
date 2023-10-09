using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Il nome della categoria è obbligatorio.")]
        [StringLength(50, ErrorMessage = "Il nome della categoria non può essere superiore a 50 caratteri.")]
        public string Name { get; set; }

        public Category() { }

        public List<Pizza>? pizzas { get; set; }
    }
}
