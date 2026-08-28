using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppTaskZero.Models;

public partial class DbTasksContext : DbContext
{
    public DbTasksContext()
    {
    }

    public DbTasksContext(DbContextOptions<DbTasksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Funcionario> Funcionarios { get; set; }

    public virtual DbSet<Gerente> Gerentes { get; set; }

    public virtual DbSet<Incidente> Incidentes { get; set; }

    public virtual DbSet<Tarefa> Tarefas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConexaoSqlServer");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__Funciona__06370DAD450DD2F8");

            entity.ToTable("Funcionario");

            entity.Property(e => e.Cargo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GerenteId).HasColumnName("GerenteID");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Gerente).WithMany(p => p.Funcionarios)
                .HasForeignKey(d => d.GerenteId)
                .HasConstraintName("FK_Gerente_Funcionario");
        });

        modelBuilder.Entity<Gerente>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__Gerente__06370DAD834589A1");

            entity.ToTable("Gerente");

            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Setor)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Incidente>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__Incident__06370DAD9A68D506");

            entity.ToTable("Incidente");

            entity.Property(e => e.DataAbertura)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DataResolucao).HasColumnType("datetime");
            entity.Property(e => e.Descricao)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Prioridade)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StatusIncidente)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Funcionario).WithMany(p => p.Incidentes)
                .HasForeignKey(d => d.FuncionarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Incidente_Funcionario");

            entity.HasOne(d => d.Tarefa).WithMany(p => p.Incidentes)
                .HasForeignKey(d => d.TarefaId)
                .HasConstraintName("FK_Incidente_Tarefa");
        });

        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__Tarefa__06370DAD8AE6BA88");

            entity.ToTable("Tarefa");

            entity.Property(e => e.DataCancelada).HasColumnType("datetime");
            entity.Property(e => e.DataFinalizada).HasColumnType("datetime");
            entity.Property(e => e.DataIniciada).HasColumnType("datetime");
            entity.Property(e => e.DataPlanejada).HasColumnType("datetime");
            entity.Property(e => e.Descricao)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Prazo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StatusTarefa)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Funcionario).WithMany(p => p.Tarefas)
                .HasForeignKey(d => d.FuncionarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tarefa_Funcionario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
