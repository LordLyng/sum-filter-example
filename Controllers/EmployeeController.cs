using AllocationIssue.Data;
using AllocationIssue.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AllocationIssue.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public EmployeeController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EmployeeDto>> GetAvailableEmployees([FromQuery] long? ts, [FromQuery] double minAvailableHours = 1)
        {
            ts ??= DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var context = _contextFactory.CreateDbContext();

            var availableEmployees = context.Employees.Include(e => e.Allocations)
                .Where(e => e.WorkHours -
                            e.Allocations.Where(pa => pa.Start < ts && pa.End > ts).Sum(pa => pa.HoursPerWeek) >=
                            minAvailableHours);

            var res = new List<EmployeeDto>();
            foreach (var availableEmployee in availableEmployees)
            {
                var diff = availableEmployee.WorkHours - availableEmployee.Allocations.Where(a => a.Start < ts && a.End > ts)
                    .Sum(a => a.HoursPerWeek);

                res.Add(new EmployeeDto
                {
                    Id = availableEmployee.Id,
                    Name = availableEmployee.Name,
                    WorkHours = availableEmployee.WorkHours,
                    Available = diff > 0,
                    AvailableHours = diff > 0 ? diff : 0
                });
            }

            return Ok(res);
        }
    }
}
