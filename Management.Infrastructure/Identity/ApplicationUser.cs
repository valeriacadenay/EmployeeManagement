using Microsoft.AspNetCore.Identity;

namespace Management.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }  // Ya tenías en Employee, mantengo aquí también.
    public string Position { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateOnly HireDate { get; set; }
    public string State { get; set; } = string.Empty;
    public string EducationLevel { get; set; } = string.Empty;
    public string ProfessionalProfile { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
}