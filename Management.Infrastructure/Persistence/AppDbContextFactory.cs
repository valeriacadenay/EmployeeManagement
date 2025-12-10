using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Management.Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder <AppDbContext>();
        // Prefer env var when running design-time tools
        var conn = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                   ?? Environment.GetEnvironmentVariable("DEFAULT_CONNECTION_STRING")
                   ?? "Host=localhost;Port=5432;Database=employee_db;Username=employee_admin;Password=EmployeeAdmin_2025!";
        optionsBuilder.UseNpgsql(conn);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        return new AppDbContext(optionsBuilder.Options);
    }
}
