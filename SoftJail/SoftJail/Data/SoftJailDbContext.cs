using SoftJail.Data.Models;

namespace SoftJail.Data
{
    using Microsoft.EntityFrameworkCore;

    public class SoftJailDbContext : DbContext
    {
        public SoftJailDbContext()
        {
        }

        public SoftJailDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Cell> Cells { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Mail> Mails { get; set; }

        public DbSet<Officer> Officers { get; set; }

        public DbSet<OfficerPrisoner> OfficersPrisoners { get; set; }

        public DbSet<Prisoner> Prisoners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OfficerPrisoner>()
                .HasKey(op => new { op.OfficerId, op.PrisonerId });

            builder.Entity<Prisoner>()
                .HasMany(p => p.PrisonerOfficers)
                .WithOne(op => op.Prisoner)
                .HasForeignKey(op => op.PrisonerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Officer>()
                .HasMany(o => o.OfficerPrisoners)
                .WithOne(op => op.Officer)
                .HasForeignKey(op => op.OfficerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Prisoner>()
                .HasMany(p => p.Mails)
                .WithOne(m => m.Prisoner)
                .HasForeignKey(m => m.PrisonerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Department>()
                .HasMany(d => d.Cells)
                .WithOne(c => c.Department)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Department>()
                .HasMany(d => d.Officers)
                .WithOne(o => o.Department)
                .HasForeignKey(o => o.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}