using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace api
{
    public partial class RailroadMapDbContext : DbContext
    {
        public RailroadMapDbContext()
        {
        }

        public RailroadMapDbContext(DbContextOptions<RailroadMapDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<NeighbourMap> NeighbourMaps { get; set; } = null!;
        public virtual DbSet<NodePost> NodePosts { get; set; } = null!;
        public virtual DbSet<PointsInLine> PointsInLines { get; set; } = null!;
        public virtual DbSet<RailroadLine> RailroadLines { get; set; } = null!;
        public virtual DbSet<RailroadPoint> RailroadPoints { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
            }
        }

        public static string ConnectionString { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb3_polish_ci")
                .HasCharSet("utf8mb3");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Idcategory)
                    .HasName("PRIMARY");

                entity.ToTable("category");

                entity.HasIndex(e => e.Discriminant, "discriminant")
                    .IsUnique();

                entity.Property(e => e.Idcategory)
                    .HasColumnType("int(11)")
                    .HasColumnName("idcategory");

                entity.Property(e => e.Discriminant)
                    .HasMaxLength(4)
                    .HasColumnName("discriminant");

                entity.Property(e => e.Fullname)
                    .HasColumnType("text")
                    .HasColumnName("fullname");
            });

            modelBuilder.Entity<NeighbourMap>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("neighbour_map");

                entity.Property(e => e.Distance).HasColumnName("distance");

                entity.Property(e => e.KilometerA).HasColumnName("kilometer_a");

                entity.Property(e => e.KilometerB).HasColumnName("kilometer_b");

                entity.Property(e => e.Linenr)
                    .HasColumnType("int(11)")
                    .HasColumnName("linenr");

                entity.Property(e => e.PostA)
                    .HasColumnType("int(11)")
                    .HasColumnName("post_a");

                entity.Property(e => e.PostB)
                    .HasColumnType("int(11)")
                    .HasColumnName("post_b");
            });

            modelBuilder.Entity<NodePost>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("node_posts");

                entity.Property(e => e.Postid)
                    .HasColumnType("int(11)")
                    .HasColumnName("postid");
            });

            modelBuilder.Entity<PointsInLine>(entity =>
            {
                entity.HasKey(e => new { e.Linenr, e.Postid })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("points_in_line");

                entity.HasIndex(e => e.Postid, "FK_PIL_RP");

                entity.Property(e => e.Linenr)
                    .HasColumnType("int(11)")
                    .HasColumnName("linenr");

                entity.Property(e => e.Postid)
                    .HasColumnType("int(11)")
                    .HasColumnName("postid");

                entity.Property(e => e.Kilometer).HasColumnName("kilometer");

                entity.HasOne(d => d.LinenrNavigation)
                    .WithMany(p => p.PointsInLines)
                    .HasForeignKey(d => d.Linenr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PIL_RL");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PointsInLines)
                    .HasForeignKey(d => d.Postid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PIL_RP");
            });

            modelBuilder.Entity<RailroadLine>(entity =>
            {
                entity.HasKey(e => e.Linenr)
                    .HasName("PRIMARY");

                entity.ToTable("railroad_lines");

                entity.Property(e => e.Linenr)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("linenr");

                entity.Property(e => e.Linename)
                    .HasColumnType("text")
                    .HasColumnName("linename");
            });

            modelBuilder.Entity<RailroadPoint>(entity =>
            {
                entity.HasKey(e => e.Postid)
                    .HasName("PRIMARY");

                entity.ToTable("railroad_points");

                entity.HasIndex(e => e.Idcategory, "FK_RP_C");

                entity.Property(e => e.Postid)
                    .HasColumnType("int(11)")
                    .HasColumnName("postid");

                entity.Property(e => e.Idcategory)
                    .HasColumnType("int(11)")
                    .HasColumnName("idcategory");

                entity.Property(e => e.Loadingpoint).HasColumnName("loadingpoint");

                entity.Property(e => e.Platform).HasColumnName("platform");

                entity.Property(e => e.Postname)
                    .HasMaxLength(256)
                    .HasColumnName("postname");

                entity.Property(e => e.Requeststop).HasColumnName("requeststop");

                entity.HasOne(d => d.IdcategoryNavigation)
                    .WithMany(p => p.RailroadPoints)
                    .HasForeignKey(d => d.Idcategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RP_C");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
