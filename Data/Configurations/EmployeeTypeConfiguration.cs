using AllocationIssue.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AllocationIssue.Data.Configurations
{
    public class EmployeeTypeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasMaxLength(36);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.WorkHours).IsRequired();

            builder.HasMany(e => e.Allocations).WithOne(a => a.Employee).HasForeignKey(a => a.EmployeeId);
        }
    }
}
