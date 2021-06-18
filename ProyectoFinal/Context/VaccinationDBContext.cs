using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ProyectoFinal.VaccinationDB
{
    public partial class VaccinationDBContext : DbContext
    {
        public VaccinationDBContext()
        {
        }

        public VaccinationDBContext(DbContextOptions<VaccinationDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Cabin> Cabins { get; set; }
        public virtual DbSet<Citizen> Citizens { get; set; }
        public virtual DbSet<CitizenxEmployee> CitizenxEmployees { get; set; }
        public virtual DbSet<Disease> Diseases { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Registry> Registries { get; set; }
        public virtual DbSet<SideEffect> SideEffects { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<Vaccine> Vaccines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=VaccinationDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("APPOINTMENT");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Dose)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("dose");

                entity.Property(e => e.DuiCitizen)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("dui_citizen")
                    .IsFixedLength(true);

                entity.Property(e => e.Hour).HasColumnName("hour");

                entity.Property(e => e.IdCabin).HasColumnName("id_cabin");

                entity.HasOne(d => d.DuiCitizenNavigation)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.DuiCitizen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__APPOINTME__dui_c__4AB81AF0");

                entity.HasOne(d => d.IdCabinNavigation)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.IdCabin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__APPOINTME__id_ca__4BAC3F29");
            });

            modelBuilder.Entity<Cabin>(entity =>
            {
                entity.ToTable("CABIN");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.IdentifierEmployee)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("identifier_employee");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("phone")
                    .IsFixedLength(true);

                entity.HasOne(d => d.IdentifierEmployeeNavigation)
                    .WithMany(p => p.Cabins)
                    .HasForeignKey(d => d.IdentifierEmployee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CABIN__identifie__398D8EEE");
            });

            modelBuilder.Entity<Citizen>(entity =>
            {
                entity.HasKey(e => e.Dui)
                    .HasName("PK__CITIZEN__D876F1BEB40AC4FB");

                entity.ToTable("CITIZEN");

                entity.Property(e => e.Dui)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("dui")
                    .IsFixedLength(true);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("fullName");

                entity.Property(e => e.Identifier)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("identifier");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("phone")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<CitizenxEmployee>(entity =>
            {
                entity.HasKey(e => new { e.DuiCitizen, e.IdentifierEmployee })
                    .HasName("PK__CITIZENx__EBF723AC00FCA4A8");

                entity.ToTable("CITIZENxEMPLOYEE");

                entity.Property(e => e.DuiCitizen)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("dui_citizen")
                    .IsFixedLength(true);

                entity.Property(e => e.IdentifierEmployee)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("identifier_employee");

                entity.HasOne(d => d.DuiCitizenNavigation)
                    .WithMany(p => p.CitizenxEmployees)
                    .HasForeignKey(d => d.DuiCitizen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CITIZENxE__dui_c__3D5E1FD2");

                entity.HasOne(d => d.IdentifierEmployeeNavigation)
                    .WithMany(p => p.CitizenxEmployees)
                    .HasForeignKey(d => d.IdentifierEmployee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CITIZENxE__ident__3E52440B");
            });

            modelBuilder.Entity<Disease>(entity =>
            {
                entity.ToTable("DISEASE");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Disease1)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("disease");

                entity.Property(e => e.DuiCitizen)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("dui_citizen")
                    .IsFixedLength(true);

                entity.HasOne(d => d.DuiCitizenNavigation)
                    .WithMany(p => p.Diseases)
                    .HasForeignKey(d => d.DuiCitizen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DISEASE__dui_cit__35BCFE0A");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Identifier)
                    .HasName("PK__EMPLOYEE__D112ED49F41CD709");

                entity.ToTable("EMPLOYEE");

                entity.Property(e => e.Identifier)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("identifier");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("fullName");

                entity.Property(e => e.IdType).HasColumnName("id_type");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.IdTypeNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.IdType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EMPLOYEE__id_typ__3A81B327");
            });

            modelBuilder.Entity<Registry>(entity =>
            {
                entity.ToTable("REGISTRY");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Hour).HasColumnName("hour");

                entity.Property(e => e.IdCabin).HasColumnName("id_cabin");

                entity.Property(e => e.IdentifierEmployee)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("identifier_employee");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("phone")
                    .IsFixedLength(true);

                entity.HasOne(d => d.IdCabinNavigation)
                    .WithMany(p => p.Registries)
                    .HasForeignKey(d => d.IdCabin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REGISTRY__id_cab__3C69FB99");

                entity.HasOne(d => d.IdentifierEmployeeNavigation)
                    .WithMany(p => p.Registries)
                    .HasForeignKey(d => d.IdentifierEmployee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REGISTRY__identi__3B75D760");
            });

            modelBuilder.Entity<SideEffect>(entity =>
            {
                entity.ToTable("SIDE_EFFECT");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdVaccine).HasColumnName("id_vaccine");

                entity.Property(e => e.SideEffect1)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("side_effect");

                entity.HasOne(d => d.IdVaccineNavigation)
                    .WithMany(p => p.SideEffects)
                    .HasForeignKey(d => d.IdVaccine)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SIDE_EFFE__id_va__38996AB5");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("TYPE");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Type1)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<Vaccine>(entity =>
            {
                entity.ToTable("VACCINE");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Dose)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("dose");

                entity.Property(e => e.DuiCitizen)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("dui_citizen")
                    .IsFixedLength(true);

                entity.Property(e => e.Hour).HasColumnName("hour");

                entity.Property(e => e.IdentifierEmployee)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("identifier_employee");

                entity.HasOne(d => d.DuiCitizenNavigation)
                    .WithMany(p => p.Vaccines)
                    .HasForeignKey(d => d.DuiCitizen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VACCINE__dui_cit__36B12243");

                entity.HasOne(d => d.IdentifierEmployeeNavigation)
                    .WithMany(p => p.Vaccines)
                    .HasForeignKey(d => d.IdentifierEmployee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VACCINE__identif__37A5467C");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
