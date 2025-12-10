namespace Management.Application.Employee.Dtos;

public class EmployeeListDto
{
    public Guid Id { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
}
