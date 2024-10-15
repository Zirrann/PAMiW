using Microsoft.AspNetCore.Mvc;
using SimpleCalculator.Models;

namespace SimpleCalculator.Controllers
{
	public class CalculatorController : Controller
	{
		// Akcja GET do wyœwietlania formularza
		public ActionResult Index()
		{
			return View();
		}

		// Akcja POST do obliczenia wyniku
		[HttpPost]
		public ActionResult Index(CalculatorModel model)
		{
			// Obliczenie sumy i przypisanie wyniku do modelu
			model.Result = model.Number1 + model.Number2;

			// Przekazanie modelu do widoku
			return View(model);
		}
	}
}
