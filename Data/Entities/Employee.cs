using System;
using System.Collections.Generic;

namespace AllocationIssue.Data.Entities
{
    public class Employee
    {
        public Employee()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public double WorkHours { get; set; }
        public bool Available { get; set; }
        public double AvailableHours { get; set; }

        public ICollection<Allocation> Allocations { get; set; }
    }
}
