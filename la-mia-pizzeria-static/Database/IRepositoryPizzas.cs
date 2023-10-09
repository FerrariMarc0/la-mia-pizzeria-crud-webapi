using la_mia_pizzeria_static.Models;

namespace la_mia_pizzeria_static.Database
{
    public interface IRepositoryPizzas
    {
        public Pizza GetPizzaById(int id);
        public List<Pizza> GetAllPizzas();
        public List<Pizza> GetPizzasByName(string name);
        public bool AddPizza(Pizza pizzaToAdd);
        public bool UpdatePizza(int id, Pizza updatedPizza);
        public bool DeletePizza(int id);
    }
}
