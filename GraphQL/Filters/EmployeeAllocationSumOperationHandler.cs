using AllocationIssue.Data.Entities;
using HotChocolate.Configuration;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Filters.Expressions;
using HotChocolate.Language;
using HotChocolate.Language.Visitors;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace AllocationIssue.GraphQL.Filters
{
    public class EmployeeAllocationSumOperationHandler : FilterOperationHandler<QueryableFilterContext, Expression>
    {
        public override bool CanHandle(ITypeCompletionContext context, IFilterInputTypeDefinition typeDefinition,
            IFilterFieldDefinition fieldDefinition)
        {
            return context.Type is IListFilterInputType &&
                   fieldDefinition is FilterOperationFieldDefinition { Id: CustomFilterOperations.Sum };
        }

        public override bool TryHandleEnter(QueryableFilterContext context, IFilterField field, ObjectFieldNode node, [NotNullWhen(true)] out ISyntaxVisitorAction? action)
        {
            var ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var property = context.GetInstance();

            Expression<Func<ICollection<Allocation>, double>> expression = allocations => allocations
                .Where(allocation => allocation.Start < ts && allocation.End > ts)
                .Sum(allocation => allocation.HoursPerWeek);
            var invoke = Expression.Invoke(expression, property);
            context.PushInstance(invoke);
            action = SyntaxVisitor.Continue;
            return true;
        }
    }
}
