using la_mia_pizzeria_static.CustomLoggers;
using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.Models.Database_models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace la_mia_pizzeria_static.Controllers
{
    [Authorize(Roles = "USER, ADMIN")]
    public class PizzaController : Controller
    {
        private ICustomLogger _myLogger;
        private PizzaContext _myDatabase;
        public PizzaController(ICustomLogger logger, PizzaContext db)
        {
            _myLogger = logger;
            _myDatabase = db;
        }

        public IActionResult Index()
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Pizza -> Index");
           
            List<Pizza> pizzas = _myDatabase.Pizzas.ToList<Pizza>();
            return View("Admin", pizzas);
            
            
        }

        public IActionResult Details(int id)
        {
            
            Pizza? foundedPizza = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if(foundedPizza == null)
            {
                return NotFound($"Nessun risultato trovato con questo id: {id}");
            }
            else
            {
                return View("Details", foundedPizza);
            }
                
            
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel data)
        {
            if(!ModelState.IsValid)
            {
                List<Category> categories = _myDatabase.Categories.ToList();
                data.Categories = categories;

                List<SelectListItem> allIngredientsSelectList = new List<SelectListItem>();
                List<Ingredient> databaseAllIngredients = _myDatabase.Ingredients.ToList();

                foreach (Ingredient ingredient in databaseAllIngredients)
                {
                    allIngredientsSelectList.Add(new SelectListItem { Text = ingredient.Name, Value = ingredient.Id.ToString() });
                }
                data.Ingredients = allIngredientsSelectList;

                return View("Create", data);
            }

            data.Pizza.Ingredients = new List<Ingredient>();

            if(data.SelectedIngredientsId != null)
            {
                foreach(string ingredientSelectedId in data.SelectedIngredientsId)
                {
                    int intIngredientSelectedId = int.Parse(ingredientSelectedId);
                    Ingredient? ingredientInDb = _myDatabase.Ingredients.Where(ingredient => ingredient.Id == intIngredientSelectedId).FirstOrDefault();

                    if(ingredientInDb != null)
                    {
                        data.Pizza.Ingredients.Add(ingredientInDb);
                    }
                }
            }
            
            _myDatabase.Pizzas.Add(data.Pizza);
            _myDatabase.SaveChanges();

            return RedirectToAction("Index");
            
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            List<Category> categories = _myDatabase.Categories.ToList();
            List<SelectListItem> allIngredientsSelectList = new List<SelectListItem>();
            List<Ingredient> databaseAllIngredients = _myDatabase.Ingredients.ToList();
            
            foreach(Ingredient ingredient in databaseAllIngredients)
            {
                allIngredientsSelectList.Add(new SelectListItem { Text = ingredient.Name, Value = ingredient.Id.ToString() });
            }

            PizzaFormModel model = new PizzaFormModel { Pizza = new Pizza(), Categories = categories, Ingredients = allIngredientsSelectList};

            return View("Create", model);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            
            Pizza? pizzaToEdit = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (pizzaToEdit == null)
            {
                return NotFound("La pizza che vuoi modificare non è stata trovata");
            }
            else
            {
                List<Category> categories = _myDatabase.Categories.ToList();
                List<Ingredient> dbIngredientList = _myDatabase.Ingredients.ToList();
                List<SelectListItem> selectListItems = new List<SelectListItem>();

                foreach(Ingredient ingredient in dbIngredientList)
                {
                    selectListItems.Add(new SelectListItem { Value = ingredient.Id.ToString(), Text = ingredient.Name, Selected = pizzaToEdit.Ingredients.Any(ingredientAssociated => ingredientAssociated.Id == ingredient.Id) });
                }

                PizzaFormModel model = new PizzaFormModel { Pizza = pizzaToEdit, Categories = categories, Ingredients = selectListItems};

                return View("Update", model);
            }
            
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<Category> categories = _myDatabase.Categories.ToList();
                data.Categories = categories;

                List<Ingredient> dbIngredientList = _myDatabase.Ingredients.ToList();
                List<SelectListItem> selectListItems = new List<SelectListItem>();

                foreach (Ingredient ingredient in dbIngredientList)
                {
                    selectListItems.Add(new SelectListItem { Value = ingredient.Id.ToString(), Text = ingredient.Name });
                }
                data.Ingredients = selectListItems;

                return View("Update", data);
            }

           
            Pizza? pizzaToUpdate = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (pizzaToUpdate != null)
            {
                pizzaToUpdate.Ingredients.Clear();

                pizzaToUpdate.Name = data.Pizza.Name;
                pizzaToUpdate.Description = data.Pizza.Description;
                pizzaToUpdate.UrlImage = data.Pizza.UrlImage;
                pizzaToUpdate.CategoryId = data.Pizza.CategoryId;

                if (data.SelectedIngredientsId != null)
                {
                    foreach (string ingredientSelectedId in data.SelectedIngredientsId)
                    {
                        int intIngredientSelectedId = int.Parse(ingredientSelectedId);
                        Ingredient? ingredientInDb = _myDatabase.Ingredients.Where(ingredient => ingredient.Id == intIngredientSelectedId).FirstOrDefault();

                        if (ingredientInDb != null)
                        {
                            pizzaToUpdate.Ingredients.Add(ingredientInDb);
                        }
                    }
                }

                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("Mi dispiace, non è stata trovata la pizza da aggiornare");
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            
            Pizza? pizzaToDelete = _myDatabase.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaToDelete != null)
            {
                _myDatabase.Pizzas.Remove(pizzaToDelete);
                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("La pizza da eliminare non è stata trovata!");
            }
            
        }


    }
}
