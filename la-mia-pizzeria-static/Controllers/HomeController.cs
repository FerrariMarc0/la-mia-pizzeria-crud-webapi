using la_mia_pizzeria_static.CustomLoggers;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace la_mia_pizzeria_static.Controllers
{
    public class HomeController : Controller
    {
        private ICustomLogger _myLogger;

        public HomeController(ICustomLogger _logger)
        {
            _myLogger = _logger;
        }

        public IActionResult Index()
        {
            _myLogger.WriteLog("L'utente è arrivato nella pagina Home -> Index");
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}