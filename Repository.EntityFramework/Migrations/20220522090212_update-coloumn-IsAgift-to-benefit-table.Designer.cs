﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository.EntityFramework;

namespace Repository.EntityFramework.Migrations
{
    [DbContext(typeof(APPDBContext))]
    [Migration("20220522090212_update-coloumn-IsAgift-to-benefit-table")]
    partial class updatecoloumnIsAgifttobenefittable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.Benefit", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("AgeSign")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("BenefitCard")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("BenefitTypeId")
                        .HasColumnType("bigint");

                    b.Property<int>("Collar")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DateToMatch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasWorkflow")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAgift")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<int>("MaritalStatus")
                        .HasColumnType("int");

                    b.Property<int>("MaxParticipant")
                        .HasColumnType("int");

                    b.Property<int>("MinParticipant")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequiredDocuments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Times")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("WorkDuration")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.Property<int>("gender")
                        .HasColumnType("int");

                    b.Property<int>("numberOfDays")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BenefitTypeId");

                    b.ToTable("Benefits");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.BenefitRequest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("BenefitId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("EmployeeId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ExpectedDateFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpectedDateTo")
                        .HasColumnType("datetime2");

                    b.Property<long?>("GroupId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RequestStatusId")
                        .HasColumnType("int");

                    b.Property<string>("RequiredDocuments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BenefitId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("GroupId");

                    b.HasIndex("RequestStatusId");

                    b.ToTable("BenefitRequests");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.BenefitType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("BenefitTypes");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.BenefitWorkflow", b =>
                {
                    b.Property<long>("BenefitId")
                        .HasColumnType("bigint");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("BenefitId", "RoleId");

                    b.ToTable("BenefitWorkflows");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.Department", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.Employee", b =>
                {
                    b.Property<long>("EmployeeNumber")
                        .HasColumnType("bigint");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Collar")
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("DepartmentId")
                        .HasColumnType("bigint");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<long>("HRId")
                        .HasColumnType("bigint");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<DateTime>("JoiningDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaritalStatus")
                        .HasColumnType("int");

                    b.Property<long>("NationalityId")
                        .HasColumnType("bigint");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<long>("PositionId")
                        .HasColumnType("bigint");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("SapNumber")
                        .HasColumnType("bigint");

                    b.Property<long?>("SupervisorId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDeptManager")
                        .HasColumnType("bit");

                    b.HasKey("EmployeeNumber");

                    b.HasIndex("CompanyId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("NationalityId");

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.HasIndex("PositionId");

                    b.HasIndex("SapNumber")
                        .IsUnique();

                    b.HasIndex("SupervisorId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.EmployeeDependent", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<long>("EmployeeNumber")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<long>("PhoneNumber")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("job")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("relation")
                        .HasColumnType("int");

                    b.HasKey("Id", "EmployeeNumber");

                    b.HasIndex("EmployeeNumber");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("EmployeeDependents");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.Group", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("BenefitId")
                        .HasColumnType("bigint");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpectedDateFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpectedDateTo")
                        .HasColumnType("datetime2");

                    b.Property<string>("GroupStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RequestStatusId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BenefitId");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasFilter("[Code] IS NOT NULL");

                    b.HasIndex("RequestStatusId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.GroupEmployee", b =>
                {
                    b.Property<long>("EmployeeId")
                        .HasColumnType("bigint");

                    b.Property<long>("GroupId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.HasKey("EmployeeId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupEmployee");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.Nationality", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Nationalities");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.Position", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.Privilege", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Privileges");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.RequestStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("RequestStatus");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.RequestWorkflow", b =>
                {
                    b.Property<long>("BenefitRequestId")
                        .HasColumnType("bigint");

                    b.Property<long>("EmployeeId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReplayDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RequestStatusId")
                        .HasColumnType("int");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("BenefitRequestId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("RequestStatusId");

                    b.ToTable("RequestWorkflows");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.Benefit", b =>
                {
                    b.HasOne("MoreForYou.Models.Models.MasterModels.BenefitType", "BenefitType")
                        .WithMany()
                        .HasForeignKey("BenefitTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BenefitType");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.BenefitRequest", b =>
                {
                    b.HasOne("MoreForYou.Models.Models.MasterModels.Benefit", "Benefit")
                        .WithMany()
                        .HasForeignKey("BenefitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoreForYou.Models.Models.MasterModels.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoreForYou.Models.Models.MasterModels.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("MoreForYou.Models.Models.MasterModels.RequestStatus", "RequestStatus")
                        .WithMany()
                        .HasForeignKey("RequestStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Benefit");

                    b.Navigation("Employee");

                    b.Navigation("Group");

                    b.Navigation("RequestStatus");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.BenefitWorkflow", b =>
                {
                    b.HasOne("MoreForYou.Models.Models.MasterModels.Benefit", "Benefit")
                        .WithMany()
                        .HasForeignKey("BenefitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Benefit");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.Employee", b =>
                {
                    b.HasOne("MoreForYou.Models.Models.MasterModels.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoreForYou.Models.Models.MasterModels.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoreForYou.Models.Models.MasterModels.Nationality", "Nationality")
                        .WithMany()
                        .HasForeignKey("NationalityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoreForYou.Models.Models.MasterModels.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoreForYou.Models.Models.MasterModels.Employee", "Supervisor")
                        .WithMany()
                        .HasForeignKey("SupervisorId");

                    b.Navigation("Company");

                    b.Navigation("Department");

                    b.Navigation("Nationality");

                    b.Navigation("Position");

                    b.Navigation("Supervisor");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.EmployeeDependent", b =>
                {
                    b.HasOne("MoreForYou.Models.Models.MasterModels.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.Group", b =>
                {
                    b.HasOne("MoreForYou.Models.Models.MasterModels.Benefit", "Benefit")
                        .WithMany()
                        .HasForeignKey("BenefitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoreForYou.Models.Models.MasterModels.RequestStatus", "RequestStatus")
                        .WithMany()
                        .HasForeignKey("RequestStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Benefit");

                    b.Navigation("RequestStatus");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.GroupEmployee", b =>
                {
                    b.HasOne("MoreForYou.Models.Models.MasterModels.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoreForYou.Models.Models.MasterModels.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("MoreForYou.Models.Models.MasterModels.RequestWorkflow", b =>
                {
                    b.HasOne("MoreForYou.Models.Models.MasterModels.BenefitRequest", "BenefitRequest")
                        .WithMany()
                        .HasForeignKey("BenefitRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoreForYou.Models.Models.MasterModels.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoreForYou.Models.Models.MasterModels.RequestStatus", "RequestStatus")
                        .WithMany()
                        .HasForeignKey("RequestStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BenefitRequest");

                    b.Navigation("Employee");

                    b.Navigation("RequestStatus");
                });
#pragma warning restore 612, 618
        }
    }
}
