using AllocationIssue.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AllocationIssue.Data.Configurations
{
    public class AllocationTypeConfiguration : IEntityTypeConfiguration<Allocation>
    {
        public void Configure(EntityTypeBuilder<Allocation> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id).HasMaxLength(36);
            builder.Property(a => a.HoursPerWeek).IsRequired();
            builder.Property(a => a.Start).IsRequired();
            builder.Property(a => a.End).IsRequired();

            builder.HasOne(a => a.Employee).WithMany(e => e.Allocations).HasForeignKey(a => a.EmployeeId);
            builder.HasOne(a => a.Task).WithMany(e => e.Allocations).HasForeignKey(a => a.TaskId);
        }
    }
}
