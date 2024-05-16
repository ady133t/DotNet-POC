using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace My_Dashboard.Models.DB
{
    public partial class MachineUtilizationContext : DbContext
    {
        public MachineUtilizationContext()
        {
        }

        public MachineUtilizationContext(DbContextOptions<MachineUtilizationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Machine> Machines { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=MachineUtilization;Trusted_Connection=True;User=sa;Password=9499581");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Machine>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("Machine");

                entity.Property(e => e.DateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                //entity.Property(e => e.Id)
                //    .ValueGeneratedOnAdd()
                //    .HasColumnName("ID");

                entity.Property(e => e.MachineId)
                    .HasMaxLength(50)
                    .HasColumnName("MachineID");

                entity.Property(e => e.IsActive);
                   

                entity.Property(e => e.MachineName).HasMaxLength(50);
            });

            modelBuilder.Entity<Test>(entity => {

                entity.HasKey(e => e.Id);

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
