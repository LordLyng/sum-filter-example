using AllocationIssue.Data;
using AllocationIssue.Data.Entities;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllocationIssue.GraphQL
{
    public class EmployeeType : ObjectType<Employee>
    {
        protected override void Configure(IObjectTypeDescriptor<Employee> descriptor)
        {
            descriptor
                .Field(e => e.WorkHours)
                .IsProjected(true);

            descriptor
                .Field("available")
                .Type<NonNullType<BooleanType>>()
                .UseDbContext<ApplicationDbContext>()
                .ResolveWith<EmployeeResolvers>(resolver => resolver.GetAvailable(default!, default!, default));

            descriptor
                .Field("availableHours")
                .Type<NonNullType<FloatType>>()
                .UseDbContext<ApplicationDbContext>()
                .ResolveWith<EmployeeResolvers>(resolver => resolver.GetAvailableHours(default!, default!, default));
        }

        private class EmployeeResolvers
        {
            public async Task<bool> GetAvailable(
                Employee employee,
                [ScopedService] ApplicationDbContext dbContext,
                CancellationToken cancellationToken)
            {
                var ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                return (await dbContext.Allocations.Where(a => a.EmployeeId == employee.Id && a.Start < ts && a.End > ts)
                    .SumAsync(a => a.HoursPerWeek, cancellationToken)) < employee.WorkHours;
            }

            public async Task<double> GetAvailableHours(
                Employee employee,
                [ScopedService] ApplicationDbContext dbContext,
                CancellationToken cancellationToken)
            {
                var ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                var diff = employee.WorkHours - await dbContext.Allocations
                    .Where(a => a.EmployeeId == employee.Id && a.Start < ts && a.End > ts)
                    .SumAsync(a => a.HoursPerWeek, cancellationToken);

                return diff > 0 ? diff : 0;
            }
        }
    }
}
