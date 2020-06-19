using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Cw10.Models
{
    public partial class uniContext : DbContext
    {
        public uniContext()
        {
        }

        public uniContext(DbContextOptions<uniContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Enrollment> Enrollment { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Studies> Studies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(System.IO.File.ReadLines("auth\\pg.cstr").First());    // secret gitignored file
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.Idenrollment)
                    .HasName("enrollment_pk");

                entity.ToTable("enrollment");

                entity.Property(e => e.Idenrollment)
                    .HasColumnName("idenrollment")
                    .HasDefaultValueSql("nextval('idenrollseq'::regclass)");

                entity.Property(e => e.Idstudy).HasColumnName("idstudy");

                entity.Property(e => e.Semester).HasColumnName("semester");

                entity.Property(e => e.Startdate)
                    .HasColumnName("startdate")
                    .HasColumnType("date");

                entity.HasOne(d => d.IdstudyNavigation)
                    .WithMany(p => p.Enrollment)
                    .HasForeignKey(d => d.Idstudy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("enrollment_studies");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Indexnumber)
                    .HasName("student_pk");

                entity.ToTable("student");

                entity.Property(e => e.Indexnumber)
                    .HasColumnName("indexnumber")
                    .HasMaxLength(100);

                entity.Property(e => e.Birthdate)
                    .HasColumnName("birthdate")
                    .HasColumnType("date");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasColumnName("firstname")
                    .HasMaxLength(100);

                entity.Property(e => e.Idenrollment).HasColumnName("idenrollment");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(256);

                entity.Property(e => e.Role)
                    .HasColumnName("role")
                    .HasMaxLength(16);

                entity.Property(e => e.Salt)
                    .HasColumnName("salt")
                    .HasMaxLength(256);

                entity.HasOne(d => d.IdenrollmentNavigation)
                    .WithMany(p => p.Student)
                    .HasForeignKey(d => d.Idenrollment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("student_enrollment");
            });

            modelBuilder.Entity<Studies>(entity =>
            {
                entity.HasKey(e => e.Idstudy)
                    .HasName("studies_pk");

                entity.ToTable("studies");

                entity.Property(e => e.Idstudy)
                    .HasColumnName("idstudy")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
