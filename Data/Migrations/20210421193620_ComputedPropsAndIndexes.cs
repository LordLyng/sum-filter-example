using Microsoft.EntityFrameworkCore.Migrations;

namespace AllocationIssue.Data.Migrations
{
    public partial class ComputedPropsAndIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE FUNCTION [dbo].[AllocSumForEmployee] (@id nvarchar(36))
                RETURNS Float
                AS 
                BEGIN
                RETURN 
                    (SELECT COALESCE(SUM([HoursPerWeek]), 0) FROM [Allocations]
                        WHERE 
                            [Start] < DATEDIFF_BIG(MILLISECOND,'1970-01-01 00:00:00.000', SYSUTCDATETIME()) AND 
                            [End] > DATEDIFF_BIG(MILLISECOND,'1970-01-01 00:00:00.000', SYSUTCDATETIME()) AND
                            [EmployeeId] = @id)
                END
            ");

            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "Employees",
                type: "bit",
                nullable: false,
                computedColumnSql: "CASE WHEN [WorkHours] > [dbo].[AllocSumForEmployee] ([Id]) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END",
                stored: false);

            migrationBuilder.AddColumn<double>(
                name: "AvailableHours",
                table: "Employees",
                type: "float",
                nullable: false,
                computedColumnSql: "IIF([WorkHours] - [dbo].[AllocSumForEmployee] ([Id]) > 0, [WorkHours] - [dbo].[AllocSumForEmployee] ([Id]), 0)",
                stored: false);

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_End",
                table: "Allocations",
                column: "End");

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_Start",
                table: "Allocations",
                column: "Start");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Allocations_End",
                table: "Allocations");

            migrationBuilder.DropIndex(
                name: "IX_Allocations_Start",
                table: "Allocations");

            migrationBuilder.DropColumn(
                name: "Available",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AvailableHours",
                table: "Employees");

            migrationBuilder.Sql("DROP FUNCTION [dbo].[AllocSumForEmployee]");
        }
    }
}
