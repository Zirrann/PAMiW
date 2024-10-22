using Microsoft.AspNetCore.Mvc;
using SimpleCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using SimpleCalculator.Models;
using System.Text.Json;

namespace SimpleCalculator.Controllers
{
    public class QuotesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
