using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAppBenchAssignment.Data;
using MyAppBenchAssignment.Models;

namespace MyAppBenchAssignment.Pages.Employees;

public class CreateModel(AppDbContext context) : PageModel
{
    [BindProperty]
    public Employee Employee { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        context.Employees.Add(Employee);
        await context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
