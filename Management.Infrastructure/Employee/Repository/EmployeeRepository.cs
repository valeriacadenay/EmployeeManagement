using Management.Application.Employee.Repository;
using Management.Domain.Entities;
using Management.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Management.Infrastructure.Employee.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _db;

    public EmployeeRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Domain.Entities.Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _db.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<Domain.Entities.Employee?> GetByDocumentAsync(string documentNumber, CancellationToken cancellationToken = default) =>
        await _db.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.DocumentNumber == documentNumber, cancellationToken);

    public async Task<Domain.Entities.Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await _db.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Email == email, cancellationToken);

    public async Task<IReadOnlyList<Domain.Entities.Employee>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _db.Employees.AsNoTracking().OrderBy(e => e.LastName).ToListAsync(cancellationToken);

    public async Task AddAsync(Domain.Entities.Employee employee, CancellationToken cancellationToken = default) =>
        await _db.Employees.AddAsync(employee, cancellationToken);

    public void Update(Domain.Entities.Employee employee) => _db.Employees.Update(employee);

    public void Remove(Domain.Entities.Employee employee) => _db.Employees.Remove(employee);

    public async Task<bool> ExistsByDocumentAsync(string documentNumber, CancellationToken cancellationToken = default) =>
        await _db.Employees.AnyAsync(e => e.DocumentNumber == documentNumber, cancellationToken);

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await _db.Employees.AnyAsync(e => e.Email == email, cancellationToken);
}
