using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Techzar.Models
{
    public partial class PostgresContext : DbContext
    {
        public PostgresContext()
        {
        }

        public PostgresContext(DbContextOptions<PostgresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Courseid).HasName("course_pkey");

                entity.ToTable("course");

                entity.Property(e => e.Courseid).HasColumnName("courseid");
                entity.Property(e => e.Coursename)
                    .HasMaxLength(100)
                    .HasColumnName("coursename");
                entity.Property(e => e.Departmentid).HasColumnName("departmentid");

                entity.HasOne(d => d.Department).WithMany(p => p.Courses)
                    .HasForeignKey(d => d.Departmentid)
                    .HasConstraintName("course_departmentid_fkey");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Departmentid).HasName("department_pkey");

                entity.ToTable("department");

                entity.Property(e => e.Departmentid).HasColumnName("departmentid");
                entity.Property(e => e.Departmentname)
                    .HasMaxLength(100)
                    .HasColumnName("departmentname");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Studentid).HasName("student_pkey");

                entity.ToTable("student");

                entity.Property(e => e.Studentid).HasColumnName("studentid");
                entity.Property(e => e.Courseid).HasColumnName("courseid");
                entity.Property(e => e.Departmentid).HasColumnName("departmentid");
                entity.Property(e => e.Studentname)
                    .HasMaxLength(100)
                    .HasColumnName("studentname");

                entity.HasOne(d => d.Course).WithMany(p => p.Students)
                    .HasForeignKey(d => d.Courseid)
                    .HasConstraintName("student_courseid_fkey");

                entity.HasOne(d => d.Department).WithMany(p => p.Students)
                    .HasForeignKey(d => d.Departmentid)
                    .HasConstraintName("student_departmentid_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
