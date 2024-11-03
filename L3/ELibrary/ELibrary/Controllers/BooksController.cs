using ELibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly JsonFileHandler _jsonFileHandler;

        public BooksController(IWebHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "App_Data", "books.json");
            _jsonFileHandler = new JsonFileHandler(filePath);
        }

        public ActionResult Index(string search = null, string sortOrder = null, string genre = null, bool? availability = null)
        {
            var books = _jsonFileHandler.LoadBooks();

            var allGenres = books.Select(b => b.Genre).Distinct().ToList();
            allGenres.Insert(0, "Wszystkie");

            ViewBag.AllGenres = allGenres;
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentGenre = genre;
            ViewBag.CurrentAvailability = availability;
            

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                books = books
                    .Where(b => b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                b.Author.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Apply genre filter
            if (!string.IsNullOrEmpty(genre) && genre != "Wszystkie")
            {
                books = books.Where(b => b.Genre == genre).ToList();
            }

            // Apply availability filter
            if (availability == true)
            {
                books = books.Where(b => b.IsAvailable).ToList();
            }

            // Define sort parameters for toggling
            ViewBag.TitleSortParam = sortOrder == "title_asc" ? "title_desc" : "title_asc";
            ViewBag.AuthorSortParam = sortOrder == "author_asc" ? "author_desc" : "author_asc";

            // Sort books based on the sortOrder parameter
            books = sortOrder switch
            {
                "title_asc" => books.OrderBy(b => b.Title).ToList(),
                "title_desc" => books.OrderByDescending(b => b.Title).ToList(),
                "author_asc" => books.OrderBy(b => b.Author).ToList(),
                "author_desc" => books.OrderByDescending(b => b.Author).ToList(),
                _ => books
            };

            return View(books);
        }

        public ActionResult Create()
        {
            var book = new Book
            {
                PublishedYear = DateTime.Now.Year 
            };
            return View(book);
        }

        [HttpPost]
        public ActionResult Create(Book book)
        {
            var books = _jsonFileHandler.LoadBooks();
            book.Id = books.Count > 0 ? books.Max(b => b.Id) + 1 : 1;
            books.Add(book);
            _jsonFileHandler.SaveBooks(books);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var book = _jsonFileHandler.LoadBooks().FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(Book book)
        {
            var books = _jsonFileHandler.LoadBooks();
            var bookToUpdate = books.FirstOrDefault(b => b.Id == book.Id);
            if (bookToUpdate != null)
            {
                bookToUpdate.Title = book.Title;
                bookToUpdate.Author = book.Author;
                bookToUpdate.Genre = book.Genre;
                bookToUpdate.PublishedYear = book.PublishedYear; 
                bookToUpdate.IsAvailable = book.IsAvailable;
                _jsonFileHandler.SaveBooks(books);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var books = _jsonFileHandler.LoadBooks();
            var bookToRemove = books.FirstOrDefault(b => b.Id == id);
            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
                _jsonFileHandler.SaveBooks(books);
            }
            return RedirectToAction("Index");
        }
    }
}
