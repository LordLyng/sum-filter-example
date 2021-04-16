using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = AllocationIssue.Data.Entities.Task;

namespace AllocationIssue.Data.Configurations
{
    public class TaskTypeConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasMaxLength(36);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(200);

            builder.HasMany(t => t.Allocations).WithOne(a => a.Task).HasForeignKey(a => a.TaskId);
        }
    }
}
