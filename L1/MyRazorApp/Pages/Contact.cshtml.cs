using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class ContactModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Message is required.")]
    public string Message { get; set; }

    public bool IsMessageSent { get; set; } = false;

    public void OnPost()
    {
        if (ModelState.IsValid)
        {
            IsMessageSent = true;
            // Opcjonalnie: tutaj mo¿na obs³u¿yæ logikê wysy³ania e-maila lub zapis do bazy danych
        }
    }
}
