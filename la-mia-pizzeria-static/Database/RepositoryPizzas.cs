using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.Models.Database_models;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Database
{
    public class RepositoryPizzas : IRepositoryPizzas
    {
        private PizzaContext _db;

        public RepositoryPizzas(PizzaContext db)
        {
            _db = db;
        }

        public Pizza GetPizzaById(int id)
        {
            Pizza? pizza = _db.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).Include(pizza => pizza.Category).FirstOrDefault();

            if (pizza != null)
            {
                return pizza;
            }
            else
            {
                throw new Exception("La pizza non è stata trovata");
            }
        }

        public List<Pizza> GetAllPizzas()
        {
            List<Pizza> pizzas = _db.Pizzas.Include(pizza => pizza.Ingredients).ToList();
            return pizzas;
        }

        public List<Pizza> GetPizzasByName(string name)
        {
            List<Pizza> foundedPizzas = _db.Pizzas.Where(pizza => pizza.Name.ToLower().Contains(name.ToLower())).ToList();
            return foundedPizzas;
        }

        public bool AddPizza(Pizza pizzaToAdd)
        {
            try
            {
                _db.Pizzas.Add(pizzaToAdd);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public bool UpdatePizza(int id, Pizza updatedPizza)
        {
            Pizza? pizzaToUpdate = _db.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (pizzaToUpdate != null)
            {
                pizzaToUpdate.Ingredients.Clear();

                pizzaToUpdate.Name = updatedPizza.Name;
                pizzaToUpdate.Description = updatedPizza.Description;
                pizzaToUpdate.UrlImage = updatedPizza.UrlImage;
                pizzaToUpdate.CategoryId = updatedPizza.CategoryId;

                _db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeletePizza(int id)
        {
            Pizza? pizzaToDelete = _db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToDelete == null)
            {
                return false;
            }

            _db.Pizzas.Remove(pizzaToDelete);
            _db.SaveChanges();

            return true;
        }
    }
}
