using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EF_DbFirsLINQCountry
{
    public partial class host1323541_pd2Context : DbContext
    {
        public static string connectionStringFile => "/ConnectionString.txt";
        public host1323541_pd2Context()
        {
        }

        public host1323541_pd2Context(DbContextOptions<host1323541_pd2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<TabCapital> TabCapitals { get; set; }
        public virtual DbSet<TabCity> TabCities { get; set; }
        public virtual DbSet<TabCountry> TabCountries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL(GetConnectionString(connectionStringFile));
                /*var str = File.ReadAllText("ConnectionString.txt");
                optionsBuilder.UseLazyLoadingProxies().UseMySQL(str);*/
            }
        }
        public static string GetConnectionString(string connectionStringFile)
        {
            var streamReader = new StreamReader(Directory.GetCurrentDirectory() + connectionStringFile);
            return streamReader.ReadToEnd();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TabCapital>(entity =>
            {
                entity.ToTable("tab_capitals");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Population)
                    .HasColumnType("int(11)")
                    .HasColumnName("population");
            });

            modelBuilder.Entity<TabCity>(entity =>
            {
                entity.ToTable("tab_cities");

                entity.HasIndex(e => e.CountryId, "country_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CountryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("country_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Population)
                    .HasColumnType("int(11)")
                    .HasColumnName("population");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TabCities)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tab_cities_ibfk_1");
            });

            modelBuilder.Entity<TabCountry>(entity =>
            {
                entity.ToTable("tab_countries");

                entity.HasIndex(e => e.CapitalId, "capital_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Area).HasColumnName("area");

                entity.Property(e => e.CapitalId)
                    .HasColumnType("int(11)")
                    .HasColumnName("capital_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.PartOfTheWorld)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("part_of_the_world");

                entity.Property(e => e.Population)
                    .HasColumnType("int(11)")
                    .HasColumnName("population");

                entity.HasOne(d => d.Capital)
                    .WithMany(p => p.TabCountries)
                    .HasForeignKey(d => d.CapitalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tab_countries_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
