using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HOPITAL2.Models;

public partial class HopitaldbContext : DbContext
{
    public HopitaldbContext()
    {
    }

    public HopitaldbContext(DbContextOptions<HopitaldbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Consultation> Consultations { get; set; }

    public virtual DbSet<DossierMedical> DossierMedicals { get; set; }

    public virtual DbSet<Examan> Examen { get; set; }

    public virtual DbSet<Medecin> Medecins { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<RendezVou> RendezVous { get; set; }

    public DbSet<PersonnelMedical> PersonnelMedicals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Update your connection string to be more secure
        optionsBuilder.UseSqlServer("Server=AMINE; Database=HOPITALDB; Trusted_Connection=True; TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Consultation>(entity =>
        {
            entity.HasKey(e => e.ConsultationId).HasName("PK__CONSULTA__5D014A78B2A045E6");

            entity.ToTable("CONSULTATION");

            entity.Property(e => e.ConsultationId).HasColumnName("ConsultationID");
            entity.Property(e => e.DateConsultation)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DossierMedicalId).HasColumnName("DossierMedicalID");
            entity.Property(e => e.Prix).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RendezVousId).HasColumnName("RendezVousID");

            entity.HasOne(d => d.DossierMedical).WithMany(p => p.Consultations)
                .HasForeignKey(d => d.DossierMedicalId)
                .HasConstraintName("FK__CONSULTAT__Dossi__4222D4EF");

            entity.HasOne(d => d.RendezVous).WithMany(p => p.Consultations)
                .HasForeignKey(d => d.RendezVousId)
                .HasConstraintName("FK__CONSULTAT__Rende__4316F928");
        });

        modelBuilder.Entity<DossierMedical>(entity =>
        {
            entity.HasKey(e => e.DossierMedicalId).HasName("PK__DOSSIER___19050E250A43B81B");

            entity.ToTable("DOSSIER_MEDICAL");

            entity.Property(e => e.DossierMedicalId).HasColumnName("DossierMedicalID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");

            entity.HasOne(d => d.Patient).WithMany(p => p.DossierMedicals)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__DOSSIER_M__Patie__3B75D760");
        });

        modelBuilder.Entity<Examan>(entity =>
        {
            entity.HasKey(e => e.ExamenId).HasName("PK__EXAMEN__AC23A2F121DD6B8E");

            entity.ToTable("EXAMEN");

            entity.Property(e => e.ExamenId).HasColumnName("ExamenID");
            entity.Property(e => e.DossierMedicalId).HasColumnName("DossierMedicalID");
            entity.Property(e => e.TypeExamen).HasMaxLength(100);

            entity.HasOne(d => d.DossierMedical).WithMany(p => p.Examen)
                .HasForeignKey(d => d.DossierMedicalId)
                .HasConstraintName("FK__EXAMEN__DossierM__4AB81AF0");
        });

        modelBuilder.Entity<Medecin>(entity =>
        {
            entity.HasKey(e => e.MedecinId).HasName("PK__MEDECIN__69D27A1B97F8013F");

            entity.ToTable("MEDECIN");

            entity.Property(e => e.MedecinId).HasColumnName("MedecinID");
            entity.Property(e => e.Nom).HasMaxLength(100);
            entity.Property(e => e.Prenom).HasMaxLength(100);
            entity.Property(e => e.Specialite).HasMaxLength(100);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__PATIENT__970EC346134C8DC9");

            entity.ToTable("PATIENT");

            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Adresse).HasMaxLength(255);
            entity.Property(e => e.Nom).HasMaxLength(100);
            entity.Property(e => e.Prenom).HasMaxLength(100);
            entity.Property(e => e.Telephone).HasMaxLength(20);
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.PrescriptionId).HasName("PK__PRESCRIP__40130812BA6828E6");

            entity.ToTable("PRESCRIPTION");

            entity.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
            entity.Property(e => e.ConsultationId).HasColumnName("ConsultationID");
            entity.Property(e => e.Medicament).HasMaxLength(100);
            entity.Property(e => e.Posologie).HasMaxLength(255);

            entity.HasOne(d => d.Consultation).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.ConsultationId)
                .HasConstraintName("FK__PRESCRIPT__Consu__47DBAE45");
        });

        modelBuilder.Entity<RendezVou>(entity =>
        {
            entity.HasKey(e => e.RendezVousId).HasName("PK__RENDEZ_V__C4B748E78BD86480");

            entity.ToTable("RENDEZ_VOUS");

            entity.Property(e => e.RendezVousId).HasColumnName("RendezVousID");
            entity.Property(e => e.DateHeure).HasColumnType("datetime");
            entity.Property(e => e.DossierMedicalId).HasColumnName("DossierMedicalID");
            entity.Property(e => e.MedecinId).HasColumnName("MedecinID");
            entity.Property(e => e.Motif).HasMaxLength(255);
            entity.Property(e => e.Statut).HasMaxLength(50);

            entity.HasOne(d => d.DossierMedical).WithMany(p => p.RendezVous)
                .HasForeignKey(d => d.DossierMedicalId)
                .HasConstraintName("FK__RENDEZ_VO__Dossi__3E52440B");

            entity.HasOne(d => d.Medecin).WithMany(p => p.RendezVous)
                .HasForeignKey(d => d.MedecinId)
                .HasConstraintName("FK__RENDEZ_VO__Medec__3F466844");
        });

        modelBuilder.Entity<PersonnelMedical>(entity =>
        {
            entity.ToTable("PERSONNEL_MEDICAL");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
