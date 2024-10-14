using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }

        // Obs≥uga øπdania Ajax
        public IActionResult OnGetGetMessage()
        {
            var message = new { message = "To jest odpowiedü z serwera!" };
            return new JsonResult(message);
        }
    }
}
