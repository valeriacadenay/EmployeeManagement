
namespace Management.Application.Employee.Repository;

public interface IEmployeeRepository
{
    Task<Domain.Entities.Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Domain.Entities.Employee?> GetByDocumentAsync(string documentNumber, CancellationToken cancellationToken = default);
    Task<Domain.Entities.Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Domain.Entities.Employee>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Domain.Entities.Employee employee, CancellationToken cancellationToken = default);
    void Update(Domain.Entities.Employee employee);
    void Remove(Domain.Entities.Employee employee);
    Task<bool> ExistsByDocumentAsync(string documentNumber, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    
}
