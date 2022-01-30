using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UserCommand.Model
{
    public partial class UserCommandDBContext : DbContext
    {
        public UserCommandDBContext()
        {
        }

        public UserCommandDBContext(DbContextOptions<UserCommandDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EventStore> EventStore { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnectionString"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventStore>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AggregateId).HasColumnName("AggregateID");

                entity.Property(e => e.Data).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Occured).HasColumnType("date");

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
