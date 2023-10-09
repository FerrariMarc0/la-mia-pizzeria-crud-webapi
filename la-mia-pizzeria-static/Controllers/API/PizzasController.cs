using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        private IRepositoryPizzas _repoPizzas;

        public PizzasController(IRepositoryPizzas repoPizzas)
        {
            _repoPizzas = repoPizzas;
        }

        [HttpGet]
        public IActionResult GetPizzas()
        {
            List<Pizza> pizzas = _repoPizzas.GetAllPizzas();

            return Ok(pizzas);
        }

        [HttpGet]
        public IActionResult SearchPizzas(string? search)
        {
            if(search == null)
            {
                return BadRequest(new {Message = "Non hai inserito nessuna stringa di ricerca."});
            }
            
            List<Pizza> foundedPizzas = _repoPizzas.GetPizzasByName(search);

            return Ok(foundedPizzas);  
        }

        [HttpGet("{id}")]
        public IActionResult PizzaById(int id)
        {
            Pizza pizza = _repoPizzas.GetPizzaById(id);
            
            if(pizza != null)
            {
                return Ok(pizza);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] Pizza newPizza)
        {
            try
            {
                bool result = _repoPizzas.AddPizza(newPizza);

                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }

            } catch (Exception ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Pizza updatedPizza)
        {
            bool result = _repoPizzas.UpdatePizza(id, updatedPizza);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _repoPizzas.DeletePizza(id);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
