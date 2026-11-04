using System.ComponentModel.DataAnnotations;

namespace MyAppBenchAssignment.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    [Required, StringLength(20)]
    [Display(Name = "Employee code")]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required, StringLength(255), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Designation { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}
