using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Config.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity
                    .HasMany(s => s.HomeworkSubmissions)
                    .WithOne(h => h.Student);

                entity
                    .Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode()
                    .IsRequired();

                entity
                    .Property(e => e.PhoneNumber)
                    .HasColumnType("CHAR(10)");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.CourseId);

                entity
                    .HasMany(c => c.HomeworkSubmissions)
                    .WithOne(h => h.Course);

                entity
                    .HasMany(c => c.Resources)
                    .WithOne(r => r.Course);

                entity
                    .Property(c => c.Name)
                    .HasMaxLength(80)
                    .IsUnicode()
                    .IsRequired();

                entity
                    .Property(c => c.Description)
                    .IsUnicode();
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(r => r.ResourceId);

                entity
                    .Property(c => c.Name)
                    .HasMaxLength(50)
                    .IsUnicode()
                    .IsRequired();
            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.HasKey(h => h.HomeworkId);
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(sc => new
                {
                    sc.CourseId,
                    sc.StudentId
                });
            });
        }
    }
}