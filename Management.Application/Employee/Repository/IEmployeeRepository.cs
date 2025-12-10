using Management.Domain.Entities;
namespace Management.Application.Employee.Persistence;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Employee> GetByDocumentAsync(string documentNumber, CancellationToken cancellationToken = default);
    Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
    void Update(Employee employee);
    void Remove(Employee employee);
    Task<bool> ExistsByDocumentAsync(string documentNumber, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    
}