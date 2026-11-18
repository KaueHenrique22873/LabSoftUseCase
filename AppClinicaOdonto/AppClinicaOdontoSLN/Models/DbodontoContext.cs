using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppClinicaOdontoSLN.Models;

public partial class DbodontoContext : DbContext
{
    public DbodontoContext()
    {
    }

    public DbodontoContext(DbContextOptions<DbodontoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agendamento> Agendamentos { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Procedimento> Procedimentos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConexaoSqlServer");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agendamento>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("agendamento");

            entity.Property(e => e.DataHora).HasColumnType("datetime");

            entity.HasOne(d => d.CodigoMedicoNavigation).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.CodigoMedico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_agendamento_medico");

            entity.HasOne(d => d.CodigoPacienteNavigation).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.CodigoPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_agendamento_paciente");

            entity.HasOne(d => d.CodigoProcedimentoNavigation).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.CodigoProcedimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_agendamento_procedimento");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("medico");

            entity.Property(e => e.Cro)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("paciente");

            entity.Property(e => e.Cpf)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Procedimento>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("procedimento");

            entity.Property(e => e.Descricao)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("usuario");

            entity.Property(e => e.Senha)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
