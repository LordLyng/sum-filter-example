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

            builder.Property(e => e.Available)
                .HasComputedColumnSql("CASE WHEN [WorkHours] > [dbo].[AllocSumForEmployee] ([Id]) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END", stored: false)
                .ValueGeneratedOnAddOrUpdate();
            builder.Property(e => e.AvailableHours)
                .HasComputedColumnSql("IIF([WorkHours] - [dbo].[AllocSumForEmployee] ([Id]) > 0, [WorkHours] - [dbo].[AllocSumForEmployee] ([Id]), 0)", stored: false)
                .ValueGeneratedOnAddOrUpdate();

            builder.HasMany(e => e.Allocations).WithOne(a => a.Employee).HasForeignKey(a => a.EmployeeId);
        }
    }
}
