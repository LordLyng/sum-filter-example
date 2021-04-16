using AllocationIssue.Data;
using AllocationIssue.Data.Entities;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;

namespace AllocationIssue.GraphQL
{
    public class EmployeeQuery
    {
        [UseDbContext(typeof(ApplicationDbContext))]
        [UsePaging(IncludeTotalCount = true)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Employee> GetEmployees([ScopedService] ApplicationDbContext context) => context.Employees.OrderBy(e => e.Name);
    }
}
