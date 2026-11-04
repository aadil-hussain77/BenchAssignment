using Microsoft.EntityFrameworkCore;
using MyAppBenchAssignment.Models;

namespace MyAppBenchAssignment.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(employee => employee.EmployeeId);
            entity.HasIndex(employee => employee.EmployeeCode).IsUnique();
            entity.HasIndex(employee => employee.Email).IsUnique();
            entity.HasIndex(employee => employee.Department);
            entity.Property(employee => employee.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(employee => employee.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(employee => employee.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedNever();
        });
    }

    public override int SaveChanges()
    {
        SetUpdatedTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetUpdatedTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetUpdatedTimestamps()
    {
        foreach (var entry in ChangeTracker.Entries<Employee>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.RowVersion = Guid.NewGuid().ToByteArray();
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.RowVersion = Guid.NewGuid().ToByteArray();
            }
        }
    }
}
