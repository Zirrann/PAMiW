using Microsoft.AspNetCore.Mvc;
using SimpleCalculator.Models;

public class CalculatorController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        // Tworzymy now¹ instancjê modelu, aby unikn¹æ NullReferenceException
        var model = new CalculatorModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult Index(CalculatorModel model)
    {
        // Logika obliczeñ (przyk³adowo sumowanie dwóch liczb)
        model.Result = model.Number1 + model.Number2;

        return View(model);
    }
}
