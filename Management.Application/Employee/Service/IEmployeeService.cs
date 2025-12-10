using Management.Application.Employee.Dtos;

namespace Management.Application.Employee.Service;

public interface IEmployeeService
{
    Task<IReadOnlyList<EmployeeReadDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<EmployeeReadDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<EmployeeReadDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<(bool created, string? error, EmployeeReadDto? employee)> CreateAsync(EmployeeCreateDto dto, CancellationToken cancellationToken = default);
    Task<(bool updated, string? error, EmployeeReadDto? employee)> UpdateAsync(Guid id, EmployeeUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
