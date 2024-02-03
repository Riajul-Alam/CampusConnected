﻿// <auto-generated />
using System;
using CampusConnected.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CampusConnected.Migrations
{
    [DbContext(typeof(StudentDBContext))]
    [Migration("20240108184821_studet_course_tal")]
    partial class studet_course_tal
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CampusConnected.Models.Course", b =>
                {
                    b.Property<int>("CId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CId"), 1L, 1);

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Course Code");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Name");

                    b.Property<int>("Faculty")
                        .HasColumnType("int");

                    b.HasKey("CId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("CampusConnected.Models.Department", b =>
                {
                    b.Property<int>("DId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DId"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Department Code");

                    b.Property<int>("Faculty")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Name");

                    b.Property<int?>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("DId");

                    b.HasIndex("StudentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("CampusConnected.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Faculty")
                        .HasColumnType("int");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("StudentId");

                    b.Property<string>("StudentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("CampusConnected.Models.StudentCourse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("CourseRegistration");
                });

            modelBuilder.Entity("CourseStudentCourse", b =>
                {
                    b.Property<int>("SelectedCoursesCId")
                        .HasColumnType("int");

                    b.Property<int>("StudentCoursesId")
                        .HasColumnType("int");

                    b.HasKey("SelectedCoursesCId", "StudentCoursesId");

                    b.HasIndex("StudentCoursesId");

                    b.ToTable("CourseStudentCourse");
                });

            modelBuilder.Entity("CampusConnected.Models.Department", b =>
                {
                    b.HasOne("CampusConnected.Models.Student", null)
                        .WithMany("DepartmentList")
                        .HasForeignKey("StudentId");
                });

            modelBuilder.Entity("CampusConnected.Models.StudentCourse", b =>
                {
                    b.HasOne("CampusConnected.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("CourseStudentCourse", b =>
                {
                    b.HasOne("CampusConnected.Models.Course", null)
                        .WithMany()
                        .HasForeignKey("SelectedCoursesCId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CampusConnected.Models.StudentCourse", null)
                        .WithMany()
                        .HasForeignKey("StudentCoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CampusConnected.Models.Student", b =>
                {
                    b.Navigation("DepartmentList");
                });
#pragma warning restore 612, 618
        }
    }
}
