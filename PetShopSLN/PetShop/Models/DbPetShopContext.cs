using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PetShop.Models;

public partial class DbPetShopContext : DbContext
{
    public DbPetShopContext()
    {
    }

    public DbPetShopContext(DbContextOptions<DbPetShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agendamento> Agendamentos { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<Tutor> Tutors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConexaoSqlServer");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agendamento>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("Agendamento");

            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.DataHora).HasColumnType("datetime");
            entity.Property(e => e.MotivoAgendamento)
                .HasMaxLength(140)
                .IsUnicode(false);

            entity.HasOne(d => d.CodigoPetNavigation).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.CodigoPet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agendamento_Pet");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("Pet");

            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.CodigoTutorNavigation).WithMany(p => p.Pets)
                .HasForeignKey(d => d.CodigoTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pet_Tutor");
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("Tutor");

            entity.HasIndex(e => e.Cpf, "Indice_CPF").IsUnique();

            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.Cpf)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.DataCriacao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PerfilAcesso)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("user");
            entity.Property(e => e.Senha)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
