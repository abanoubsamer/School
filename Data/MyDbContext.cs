using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using School.Models;
namespace School.Data;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options): base(options)// dh ale constractor ale hft7 beh ale conection by ale data base
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<CourseLevel> CourseLevel { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=BIBOSAMER12;User ID=sa;Password=123456;Database=School;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__C92D71879E6E94D4");

            entity.ToTable("Course");

            entity.Property(e => e.Tital).HasMaxLength(50); // Corrected 'Tital' to 'Title'

            entity.HasOne(d => d.CourseLevel)
                .WithMany()
                .HasForeignKey(d => d.LevelId)
                .HasConstraintName("FK_CourseLevel");
        });


        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__Enrollme__449AA16273D6EBCD");

            entity.ToTable("Enrollment");

            entity.Property(e => e.EnrollmentId).HasColumnName("Enrollment_id");
            entity.Property(e => e.Grade).HasColumnType("decimal(4, 1)");

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Enrollmen__Cours__286302EC");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Enrollmen__Stude__29572725");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__A2F7EDF496675619");

            entity.ToTable("Student");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });


        modelBuilder.Entity<CourseLevel>(entity =>
        {
            entity.HasKey(e => e.CourseLevelId).HasName("PK__CourseLe__A4A11C2A16C1FE21");

            entity.ToTable("CourseLevel");

            entity.Property(e => e.Level).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
