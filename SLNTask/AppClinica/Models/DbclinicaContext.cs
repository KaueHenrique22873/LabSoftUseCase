using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppClinica.Models;

public partial class DbclinicaContext : DbContext
{
    public DbclinicaContext()
    {
    }

    public DbclinicaContext(DbContextOptions<DbclinicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Consulta> Consulta { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConexaoSqlServer");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__Consulta__06370DAD117C2DDB");

            entity.Property(e => e.DataHora).HasColumnType("datetime");
            entity.Property(e => e.StatusConsulta)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Medico).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consulta_Medico");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consulta_Paciente");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__Medico__06370DAD3D01765F");

            entity.ToTable("Medico");

            entity.Property(e => e.Crm)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("CRM");
            entity.Property(e => e.Especialidade)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__Paciente__06370DAD2A2A37AF");

            entity.ToTable("Paciente");

            entity.Property(e => e.Cpf)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("CPF");
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
