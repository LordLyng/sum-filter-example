using System;

namespace AllocationIssue.Data.Entities
{
    public class Allocation
    {
        public Allocation()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public double HoursPerWeek { get; set; }
        public long Start { get; set; }
        public long End { get; set; }

        public virtual Employee Employee { get; set; }
        public string EmployeeId { get; set; }

        public virtual Task Task { get; set; }
        public string TaskId { get; set; }
    }
}
