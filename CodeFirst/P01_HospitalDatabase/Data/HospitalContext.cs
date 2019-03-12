﻿using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> PatientsMedicaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreatePatientEntity(modelBuilder);

            CreateVisitationEntity(modelBuilder);

            CreateDiagnoseEntity(modelBuilder);

            CreateMedicamentEntity(modelBuilder);

            CreatePatientMedicamentEntity(modelBuilder);

            CreateDoctorEntity(modelBuilder);
        }

        private static void CreateDoctorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Doctor>()
                .HasKey(d => d.DoctorId);

            modelBuilder
                .Entity<Doctor>()
                .HasMany(d => d.Visitations)
                .WithOne(v => v.Doctor);

            modelBuilder
                .Entity<Doctor>()
                .Property(d => d.Name)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();

            modelBuilder
                .Entity<Doctor>()
                .Property(d => d.Specialty)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();
        }

        private static void CreatePatientMedicamentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PatientMedicament>()
                .HasKey(pm => new { pm.PatientId, pm.MedicamentId });

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(pm => pm.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(pm => pm.PatientId);

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(pm => pm.Medicament)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(pm => pm.MedicamentId);
        }

        private static void CreateMedicamentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Medicament>()
                .HasKey(m => m.MedicamentId);

            modelBuilder
                .Entity<Medicament>()
                .Property(m => m.Name)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();
        }

        private static void CreateDiagnoseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Diagnose>()
                .HasKey(d => d.DiagnoseId);

            modelBuilder
                .Entity<Diagnose>()
                .Property(d => d.Name)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

            modelBuilder
                .Entity<Diagnose>()
                .Property(d => d.Comments)
                .HasMaxLength(250)
                .IsUnicode();
        }

        private static void CreateVisitationEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Visitation>()
                .HasKey(v => v.VisitationId);

            modelBuilder
                .Entity<Visitation>()
                .Property(v => v.Comments)
                .HasMaxLength(250);
        }

        private static void CreatePatientEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Patient>()
                .HasKey(p => p.PatientId);

            modelBuilder
                .Entity<Patient>()
                .HasMany(p => p.Visitations)
                .WithOne(v => v.Patient);

            modelBuilder
                .Entity<Patient>()
                .HasMany(p => p.Diagnoses)
                .WithOne(d => d.Patient);

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.LastName)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.Address)
                .HasMaxLength(250)
                .IsUnicode()
                .IsRequired();

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.Email)
                .HasMaxLength(80);
        }
    }
}