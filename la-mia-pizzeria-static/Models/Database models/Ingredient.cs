using System.Text.Json.Serialization;

namespace la_mia_pizzeria_static.Models.Database_models
{
    public class Ingredient
    {
        [JsonIgnore]
        public int Id {  get; set; }

        public string Name { get; set; }
        public List<Pizza> Pizzas { get; set; }

        public Ingredient() { }
    }
}
