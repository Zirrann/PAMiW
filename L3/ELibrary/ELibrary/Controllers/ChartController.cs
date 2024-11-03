using Microsoft.AspNetCore.Mvc;
using ELibrary.Models;
using System.Linq;

namespace ELibrary.Controllers
{
    public class ChartController : Controller
    {
        private readonly JsonFileHandler _jsonFileHandler;

        public ChartController(IWebHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "App_Data", "books.json");
            _jsonFileHandler = new JsonFileHandler(filePath);
        }

        // GET: /Chart
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetBookCountsByGenre()
        {
            var books = _jsonFileHandler.LoadBooks();

            // Grupa wszystkich książek według gatunku
            var bookCountsByGenre = books
                .GroupBy(b => b.Genre)
                .Select(g => new
                {
                    Genre = g.Key,
                    Count = g.Count(),
                    AvailableCount = g.Count(b => b.IsAvailable)
                })
                .OrderBy(g => g.Genre)
                .ToList();

            var availableBooks = bookCountsByGenre
                    .Select(g => new
                    {
                        g.Genre,
                        Count = g.AvailableCount
                    })
                    .Where(g => g.Count > 0)
                    .ToList();

            return Json(new
            {
                AllBooks = bookCountsByGenre,
                AvailableBooks = availableBooks
            });
        }
    }
}
