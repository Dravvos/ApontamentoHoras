using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApontamentoHoras.Data;

public partial class ApontamentoHorasContext : DbContext
{
    public ApontamentoHorasContext()
    {
    }

    public ApontamentoHorasContext(DbContextOptions<ApontamentoHorasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Apontamento> Apontamentos { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Projeto> Projetos { get; set; }

    public virtual DbSet<ProjetoUsuario> ProjetoUsuarios { get; set; }

    public virtual DbSet<TabelaGeral> TabelaGerals { get; set; }

    public virtual DbSet<TabelaGeralItem> TabelaGeralItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("ApontamentoHorasConn"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Apontamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Apontamento_pkey");

            entity.ToTable("Apontamento");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DataAlteracao).HasColumnType("timestamp without time zone");
            entity.Property(e => e.DataInclusao).HasColumnType("timestamp without time zone");
            entity.Property(e => e.IdTgservico).HasColumnName("IdTGServico");

            entity.HasOne(d => d.IdTgservicoNavigation).WithMany(p => p.Apontamentos)
                .HasForeignKey(d => d.IdTgservico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Apontamento_IdTGServico_fkey");

            entity.HasOne(d => d.Projeto).WithMany(p => p.Apontamentos)
                .HasForeignKey(d => d.ProjetoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Apontamento_ProjetoId_fkey");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Apontamentos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Apontamento_UsuarioId_fkey");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Projeto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Projeto_pkey");

            entity.ToTable("Projeto");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DataAlteracao).HasColumnType("timestamp without time zone");
            entity.Property(e => e.DataInclusao).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<ProjetoUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ProjetoUsuario_pkey");

            entity.ToTable("ProjetoUsuario");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DataAlteracao).HasColumnType("timestamp without time zone");
            entity.Property(e => e.DataInclusao).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Projeto).WithMany(p => p.ProjetoUsuarios)
                .HasForeignKey(d => d.ProjetoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProjetoUsuario_ProjetoId_fkey");

            entity.HasOne(d => d.Usuario).WithMany(p => p.ProjetoUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProjetoUsuario_UsuarioId_fkey");
        });

        modelBuilder.Entity<TabelaGeral>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TabelaGeral_pkey");

            entity.ToTable("TabelaGeral");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DataAlteracao).HasColumnType("timestamp without time zone");
            entity.Property(e => e.DataInclusao).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<TabelaGeralItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TabelaGeralItem_pkey");

            entity.ToTable("TabelaGeralItem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DataAlteracao).HasColumnType("timestamp without time zone");
            entity.Property(e => e.DataInclusao).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Sigla).HasMaxLength(6);

            entity.HasOne(d => d.TabelaGeral).WithMany(p => p.TabelaGeralItems)
                .HasForeignKey(d => d.TabelaGeralId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TabelaGeralItem_TabelaGeralId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
