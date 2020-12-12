using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ClosestCity.Models
{
    public partial class closestcityContext : DbContext
    {
        public closestcityContext(DbContextOptions<closestcityContext> options)
            : base(options)
        {
        }

        public virtual DbSet<WorldcitiesGeometry> WorldcitiesGeometries { get; set; }
        public virtual DbSet<Worldcity> Worldcities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("fuzzystrmatch")
                .HasPostgresExtension("postgis")
                .HasPostgresExtension("postgis_tiger_geocoder")
                .HasPostgresExtension("postgis_topology")
                .HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<WorldcitiesGeometry>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("worldcities_geometry");

                entity.Property(e => e.AdminName).HasColumnName("admin_name");

                entity.Property(e => e.Capital).HasColumnName("capital");

                entity.Property(e => e.City).HasColumnName("city");

                entity.Property(e => e.CityAscii).HasColumnName("city_ascii");

                entity.Property(e => e.Country).HasColumnName("country");

                entity.Property(e => e.Geom).HasColumnName("geom");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Iso2).HasColumnName("iso2");

                entity.Property(e => e.Iso3).HasColumnName("iso3");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Lng).HasColumnName("lng");

                entity.Property(e => e.Population).HasColumnName("population");
            });

            modelBuilder.Entity<Worldcity>(entity =>
            {
                entity.ToTable("worldcities");

                entity.HasIndex(e => e.Id, "ix_public_worldcities_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdminName).HasColumnName("admin_name");

                entity.Property(e => e.Capital).HasColumnName("capital");

                entity.Property(e => e.City).HasColumnName("city");

                entity.Property(e => e.CityAscii).HasColumnName("city_ascii");

                entity.Property(e => e.Country).HasColumnName("country");

                entity.Property(e => e.Iso2).HasColumnName("iso2");

                entity.Property(e => e.Iso3).HasColumnName("iso3");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Lng).HasColumnName("lng");

                entity.Property(e => e.Population).HasColumnName("population");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
