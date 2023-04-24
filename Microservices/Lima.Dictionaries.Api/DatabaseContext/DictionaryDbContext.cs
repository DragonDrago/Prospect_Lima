using Lima.Dictionaries.Api.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Lima.Dictionaries.Api.DatabaseContext
{
    public class DictionaryDbContext : DbContext
    {
        public DictionaryDbContext() { }

        public DictionaryDbContext(DbContextOptions options)
            : base(options)
        { 
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>(e => e.ToTable("DICT_DOCTORS"));
            modelBuilder.Entity<Organization>(e => e.ToTable("DICT_ORGANIZATIONS"));
            modelBuilder.Entity<DoctorOrganization>(e => e.ToTable("DOCTORS_ORGANIZATIONS_RELATIONS"));
            modelBuilder.Entity<ProductDomain>(e => e.ToTable("PRODUCTS"));

            //modelBuilder.Entity<Doctor>()
            //.HasMany(p => p.Organizations)
            //.WithMany(p => p.Doctors)
            //.UsingEntity<DoctorOrganization>(
            //    j => j
            //        .HasOne(pt => pt.Organization)
            //        .WithMany(p => p.DoctorOrganizations)
            //        .HasForeignKey(pt => pt.OrganizationId),
            //    j => j
            //        .HasOne(pt => pt.Doctor)
            //        .WithMany(t => t.DoctorOrganizations)
            //        .HasForeignKey(pt => pt.DoctorId),
            //    j =>
            //    {
            //        j.HasKey(t => new { t.DoctorId, t.OrganizationId });
            //    });


            //modelBuilder.Entity<DoctorOrganization>()
            //    .HasKey(t => new { t.DoctorId, t.OrganizationId });

            //modelBuilder.Entity<DoctorOrganization>()
            //    .HasOne(pt => pt.Doctor)
            //    .WithMany(p => p.DoctorOrganizations)
            //    .HasForeignKey(pt => pt.DoctorId);

            //modelBuilder.Entity<DoctorOrganization>()
            //    .HasOne(pt => pt.Organization)
            //    .WithMany(p => p.DoctorOrganizations)
            //    .HasForeignKey(pt => pt.OrganizationId);
        }

        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<DoctorOrganization>  DoctorOrganizations { get; set; }
        public virtual DbSet<ProductDomain> ProductDomains { get; set; }
    }
}
