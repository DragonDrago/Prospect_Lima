using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Lima.Orders.Api.Models
{
    public partial class LIMA_DREAM_PHARMContext : DbContext
    {
        public LIMA_DREAM_PHARMContext()
        {
        }

        public LIMA_DREAM_PHARMContext(DbContextOptions<LIMA_DREAM_PHARMContext> options)
            : base(options)
        {
            //Orders.Where(w => w.Visit.UserId == 1)
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrdersDetailing> OrdersDetailings { get; set; }
        public virtual DbSet<Visit> Visits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=LIMA_DREAM_PHARM;Connect Timeout=30;User id=lima;Password=lima");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("ORDERS");

                entity.Property(e => e.DateCreate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PrepaymentPercent).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.Visit)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.VisitId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ORDERS_VISITS");
            });

            modelBuilder.Entity<OrdersDetailing>(entity =>
            {
                entity.ToTable("ORDERS_DETAILING");

                entity.Property(e => e.DateCreate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SalePrice).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrdersDetailings)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_ORDERS_DETAILING_ORDERS");
            });

            modelBuilder.Entity<Visit>(entity =>
            {
                entity.ToTable("VISITS");

                entity.Property(e => e.Comment).HasMaxLength(4000);

                entity.Property(e => e.DateCreate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DoctorName).HasMaxLength(100);

                entity.Property(e => e.DoctorPhone).HasMaxLength(15);

                entity.Property(e => e.DoctorPosition).HasMaxLength(100);

                entity.Property(e => e.OrganizationName).HasMaxLength(300);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
