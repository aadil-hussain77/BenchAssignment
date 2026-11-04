using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyAppBenchAssignment.Data;
using MyAppBenchAssignment.Models;

namespace MyAppBenchAssignment.Pages.Employees;

public class EditModel(AppDbContext context) : PageModel
{
    [BindProperty]
    public Employee Employee { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var employee = await context.Employees.AsNoTracking().SingleOrDefaultAsync(item => item.EmployeeId == id);
        if (employee is null)
        {
            return NotFound();
        }

        Employee = employee;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var employeeToUpdate = await context.Employees.SingleOrDefaultAsync(item => item.EmployeeId == id);
        if (employeeToUpdate is null)
        {
            return NotFound();
        }

        employeeToUpdate.EmployeeCode = Employee.EmployeeCode;
        employeeToUpdate.FirstName = Employee.FirstName;
        employeeToUpdate.LastName = Employee.LastName;
        employeeToUpdate.Email = Employee.Email;
        employeeToUpdate.Department = Employee.Department;
        employeeToUpdate.Designation = Employee.Designation;
        employeeToUpdate.IsActive = Employee.IsActive;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await context.Employees.AnyAsync(item => item.EmployeeId == id))
            {
                return NotFound();
            }

            ModelState.AddModelError(string.Empty, "This employee was changed by another user. Reload the page and try again.");
            return Page();
        }

        return RedirectToPage("Index");
    }
}
