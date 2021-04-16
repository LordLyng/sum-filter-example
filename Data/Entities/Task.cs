using System;
using System.Collections.Generic;

namespace AllocationIssue.Data.Entities
{
    public class Task
    {
        public Task()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<Allocation> Allocations { get; set; }
    }
}
