using Microsoft.AspNetCore.Mvc;
using SimpleCalculator.Models;
using SimpleCalculator.Extensions;

namespace SimpleCalculator.Controllers
{
    public class TodoController : Controller
    {
        private const string SessionKey = "TodoList";

        // Pobiera listę zadań z sesji, jeśli lista nie istnieje, tworzy nową
        private List<TodoItem> GetTodoList()
        {
            var todoList = HttpContext.Session.GetObjectFromJson<List<TodoItem>>(SessionKey);
            if (todoList == null)
            {
                todoList = new List<TodoItem>();
                HttpContext.Session.SetObjectAsJson(SessionKey, todoList);
            }
            return todoList;
        }

        // Zapisuje zaktualizowaną listę zadań w sesji
        private void SaveTodoList(List<TodoItem> todoList)
        {
            HttpContext.Session.SetObjectAsJson(SessionKey, todoList);
        }

        // Strona główna listy TODO
        public IActionResult Index()
        {
            var todoList = GetTodoList();
            return View(todoList);
        }

        // Dodanie nowego zadania
        [HttpPost]
        public IActionResult Add(string task)
        {
            if (!string.IsNullOrEmpty(task))
            {
                var todoList = GetTodoList();
                var newItem = new TodoItem { Id = todoList.Count + 1, Task = task, IsCompleted = false };
                todoList.Add(newItem);
                SaveTodoList(todoList);
            }
            return RedirectToAction("Index");
        }

        // Oznaczenie zadania jako ukończone
        public IActionResult Complete(int id)
        {
            var todoList = GetTodoList();
            var item = todoList.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                item.IsCompleted = true;
                SaveTodoList(todoList);
            }
            return RedirectToAction("Index");
        }

        // Usunięcie zadania
        public IActionResult Delete(int id)
        {
            var todoList = GetTodoList();
            var item = todoList.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                todoList.Remove(item);
                SaveTodoList(todoList);
            }
            return RedirectToAction("Index");
        }
    }
}
