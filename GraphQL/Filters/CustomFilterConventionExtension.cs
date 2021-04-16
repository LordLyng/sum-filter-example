using AllocationIssue.Data.Entities;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Filters.Expressions;

namespace AllocationIssue.GraphQL.Filters
{
    public class CustomFilterConventionExtension : FilterConventionExtension
    {
        protected override void Configure(IFilterConventionDescriptor descriptor)
        {
            //var employees = new List<Employee>();
            //var ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            //double minavailable = 1;
            //employees.Where(e => 
            //    e.WorkHours -
            //    e.Allocations
            //        .Where(a => a.Start < ts && a.End > ts)
            //        .Sum(a => a.HoursPerWeek)
            //    > 0);

            ////employees(where: {allocations: {sum: {lt: {workHours}}})


            descriptor.Operation(CustomFilterOperations.Sum)
                .Name("sum");

            descriptor.Configure<ListFilterInputType<FilterInputType<Allocation>>>(descriptor =>
            {
                descriptor
                    .Operation(CustomFilterOperations.Sum)
                    .Type<ComparableOperationFilterInputType<double>>();
            });

            descriptor.AddProviderExtension(new QueryableFilterProviderExtension(
                y =>
                {
                    y.AddFieldHandler<EmployeeAllocationSumOperationHandler>();
                }));
        }
    }
}
