using AllocationIssue.Data.Entities;
using System.Collections.Generic;

namespace AllocationIssue.Dto
{
    public class EmployeeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double WorkHours { get; set; }

        public ICollection<Allocation> Allocations { get; set; }

        public bool Available { get; set; }
        public double AvailableHours { get; set; }
    }
}
