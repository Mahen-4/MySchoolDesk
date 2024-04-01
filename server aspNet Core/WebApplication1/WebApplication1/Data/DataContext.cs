using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; } 
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set;}
        public DbSet<Result> Results { get; set; }  
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Exam> Exams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>()
              .HasOne(t => t.User) 
              .WithOne(u => u.Teacher)
              .HasForeignKey<Teacher>(t => t.UserId)
              .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Exams) 
                .WithOne(e => e.Teacher)
                .HasForeignKey(e => e.TeacherId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Teacher)
                .WithMany(t => t.Lessons)
                .HasForeignKey(l => l.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.SchoolClass)
                .WithMany(sc => sc.Lessons)
                .HasForeignKey(l => l.SchoolClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exam>()
                .HasOne(e => e.SchoolClass)
                .WithMany(sc => sc.Exams)
                .HasForeignKey(e => e.SchoolClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Teacher)
                .WithMany(t => t.Exams)
                .HasForeignKey(e => e.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Result>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Results)
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Result>()
                .HasOne(r => r.Exam)
                .WithMany(e => e.Results)
                .HasForeignKey(r => r.ExamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.SchoolClass)
                .WithMany(sc => sc.Students)
                .HasForeignKey(s => s.SchoolClassId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }

    
}
