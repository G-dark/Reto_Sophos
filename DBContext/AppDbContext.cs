using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Reto_sophos2.Models;
using Task = Reto_sophos2.Models.Task;

namespace Reto_sophos2.DBContext;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Fight> Fights { get; set; }

    public virtual DbSet<Hero> Heroes { get; set; }

    public virtual DbSet<HistoryUser> HistoryUsers { get; set; }

    public virtual DbSet<Sponsor> Sponsors { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Villain> Villains { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ELI\\SQLEXPRESS;Initial Catalog=MiBaseDeDatos; User ID=sa;Password=1234567; Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fight>(entity =>
        {
            entity.HasKey(e => e.FightId).HasName("PK__fight__F3479801731A1524");

            entity.HasOne(d => d.Hero).WithMany(p => p.Fights).HasConstraintName("FK__fight__hero_ID__1AD3FDA4");

            entity.HasOne(d => d.Villain).WithMany(p => p.Fights).HasConstraintName("FK__fight__villain_I__1BC821DD");
        });

        modelBuilder.Entity<Hero>(entity =>
        {
            entity.HasKey(e => e.HeroId).HasName("PK__hero__85866D2B529017E6");
        });

        modelBuilder.Entity<HistoryUser>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__history___F3DBC573B5EFBAB6");

            entity.HasOne(d => d.UsernameNavigation).WithOne(p => p.HistoryUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__history_u__usern__29221CFB");
        });

        modelBuilder.Entity<Sponsor>(entity =>
        {
            entity.HasKey(e => e.TransId).HasName("PK__sponsor__438DA860BC951645");

            entity.HasOne(d => d.Hero).WithMany(p => p.Sponsors).HasConstraintName("FK__sponsor__hero_ID__2EDAF651");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__task__049318B5F719A462");

            entity.Property(e => e.TaskId).ValueGeneratedNever();

            entity.HasOne(d => d.EditedByNavigation).WithMany(p => p.Tasks).HasConstraintName("FK__task__edited_by__17F790F9");

            entity.HasOne(d => d.Hero).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__task__hero_ID__17036CC0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__users__F3DBC573AD78CDEB");
        });

        modelBuilder.Entity<Villain>(entity =>
        {
            entity.HasKey(e => e.VillainId).HasName("PK__villain__8D6C36020A65A65F");
        });
        modelBuilder.HasSequence<int>("task_seq")
            .StartsAt(0L)
            .HasMin(0L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
