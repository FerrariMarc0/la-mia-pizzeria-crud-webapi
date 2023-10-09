using la_mia_pizzeria_static.Models.Database_models;
using la_mia_pizzeria_static.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<Ingredient>? Ingredients { get; set; }

        [Required(ErrorMessage = "Questo campo non può essere vuoto")]
        [StringLength(50, ErrorMessage = "Il nome NON può essere piu lungo di 50 caratteri.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Questo campo NON può essere vuoto")]
        [Column(TypeName = "text")]
        [MoreThanFiveWords]
        public string Description { get; set; }

        [Required(ErrorMessage = "Questo campo NON può essere vuoto")]
        [StringLength(500, ErrorMessage = "Questo campo NON può essere piu lungo di 500 caratteri.")]
        [Url(ErrorMessage = "Questo campo deve essere un valido link url")]
        public string UrlImage { get; set; }

        [Required(ErrorMessage = "Questo campo NON può essere vuoto")]
        [Range(0.01, 10000, ErrorMessage = "Il prezzo NON può essere inferiore ad 1 centesimo")]
        public decimal Price { get; set; }


        public Pizza(string name, string description, string image ,decimal price)
        {
            Name = name;
            Description = description;
            UrlImage = image;
            Price = price;
        }
        public Pizza(){}
    }
}
