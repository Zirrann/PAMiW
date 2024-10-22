using Microsoft.AspNetCore.Mvc;
using SimpleCalculator.Models;

public class CalculatorController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        // Tworzymy now� instancj� modelu, aby unikn�� NullReferenceException
        var model = new CalculatorModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult Index(CalculatorModel model)
    {
        // Logika oblicze� (przyk�adowo sumowanie dw�ch liczb)
        model.Result = model.Number1 + model.Number2;

        return View(model);
    }
}
