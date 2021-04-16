using AllocationIssue.Data;
using AllocationIssue.Data.Entities;
using AllocationIssue.GraphQL;
using AllocationIssue.GraphQL.Filters;
using Bogus;
using Bogus.DataSets;
using HotChocolate.Data.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace AllocationIssue
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DB"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });

            services.AddGraphQLServer()
                .AddQueryType<EmployeeQuery>()
                .AddType<EmployeeType>()
                .AddProjections()
                .AddFiltering()
                .AddConvention<IFilterConvention, CustomFilterConventionExtension>()
                .AddSorting();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var contextFactory =
                    scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
                var context = contextFactory.CreateDbContext();

                context.Database.Migrate();

                if (!context.Employees.Any())
                {
                    var employeeFaker = new Faker<Employee>().UseSeed(1337);
                    employeeFaker
                        .RuleFor(e => e.Name, f => f.Name.FullName(f.PickRandom<Name.Gender>()))
                        .RuleFor(e => e.WorkHours, f => f.Random.Double(0, 40));

                    context.Employees.AddRange(employeeFaker.Generate(100));
                    context.SaveChanges();
                }

                if (!context.Tasks.Any())
                {
                    var taskFaker = new Faker<Task>().UseSeed(1337);
                    taskFaker
                        .RuleFor(t => t.Name, f => f.Commerce.ProductName());

                    context.Tasks.AddRange(taskFaker.Generate(100));
                    context.SaveChanges();
                }

                if (!context.Allocations.Any())
                {
                    var allocationFaker = new Faker<Allocation>().UseSeed(1337);
                    allocationFaker
                        .RuleFor(a => a.EmployeeId, f => f.PickRandom(context.Employees.AsEnumerable()).Id)
                        .RuleFor(a => a.TaskId, f => f.PickRandom(context.Tasks.AsEnumerable()).Id)
                        .RuleFor(a => a.Start, f =>
                            f.Date.BetweenOffset(
                                new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
                                new(2050, 1, 1, 0, 0, 0, TimeSpan.Zero)
                            ).ToUnixTimeMilliseconds())
                        .RuleFor(a => a.End, (f, a) =>
                            f.Date.BetweenOffset(
                                DateTimeOffset.FromUnixTimeMilliseconds(a.Start),
                                new(2050, 1, 1, 0, 0, 0, TimeSpan.Zero)
                            ).ToUnixTimeMilliseconds())
                        .RuleFor(a => a.HoursPerWeek, f => f.Random.Double(0, 40));

                    context.Allocations.AddRange(allocationFaker.Generate(200));
                    context.SaveChanges();
                }
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
                endpoints.MapControllers();
            });
        }
    }
}
