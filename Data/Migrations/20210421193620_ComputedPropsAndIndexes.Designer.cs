﻿// <auto-generated />
using AllocationIssue.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AllocationIssue.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210421193620_ComputedPropsAndIndexes")]
    partial class ComputedPropsAndIndexes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AllocationIssue.Data.Entities.Allocation", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("nvarchar(36)");

                    b.Property<long>("End")
                        .HasColumnType("bigint");

                    b.Property<double>("HoursPerWeek")
                        .HasColumnType("float");

                    b.Property<long>("Start")
                        .HasColumnType("bigint");

                    b.Property<string>("TaskId")
                        .HasColumnType("nvarchar(36)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("End");

                    b.HasIndex("Start");

                    b.HasIndex("TaskId");

                    b.ToTable("Allocations");
                });

            modelBuilder.Entity("AllocationIssue.Data.Entities.Employee", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<bool>("Available")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bit")
                        .HasComputedColumnSql("CASE WHEN [WorkHours] > [dbo].[AllocSumForEmployee] ([Id]) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END", false);

                    b.Property<double>("AvailableHours")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("float")
                        .HasComputedColumnSql("IIF([WorkHours] - [dbo].[AllocSumForEmployee] ([Id]) > 0, [WorkHours] - [dbo].[AllocSumForEmployee] ([Id]), 0)", false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<double>("WorkHours")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("AllocationIssue.Data.Entities.Task", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("AllocationIssue.Data.Entities.Allocation", b =>
                {
                    b.HasOne("AllocationIssue.Data.Entities.Employee", "Employee")
                        .WithMany("Allocations")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("AllocationIssue.Data.Entities.Task", "Task")
                        .WithMany("Allocations")
                        .HasForeignKey("TaskId");

                    b.Navigation("Employee");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("AllocationIssue.Data.Entities.Employee", b =>
                {
                    b.Navigation("Allocations");
                });

            modelBuilder.Entity("AllocationIssue.Data.Entities.Task", b =>
                {
                    b.Navigation("Allocations");
                });
#pragma warning restore 612, 618
        }
    }
}