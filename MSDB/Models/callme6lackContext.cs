using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MSDB.Models;

namespace MSDB
{
    public partial class callme6lackContext : DbContext
    {
        public callme6lackContext()
        {
        }

        public callme6lackContext(DbContextOptions<callme6lackContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Budget> Budget { get; set; }
        public virtual DbSet<BuyRawMaterial> BuyRawMaterial { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<FinishedProducts> FinishedProducts { get; set; }
        public virtual DbSet<GotovayaProduction> GotovayaProduction { get; set; }
        public virtual DbSet<Ingredients> Ingredients { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }
        public virtual DbSet<Produciya> Produciya { get; set; }
        public virtual DbSet<Prodution> Prodution { get; set; }
        public virtual DbSet<PurchaseOfRawMaterials> PurchaseOfRawMaterials { get; set; }
        public virtual DbSet<SaleOfProducts> SaleOfProducts { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<Salaryes> Salaryes { get; set; }
        public virtual DbSet<Years> Years { get; set; }
        public virtual DbSet<Months> Months { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=SHELOVESLOLOO\\SQLEXPRESS; Database=callme6lack;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Budget>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SumBudget).HasColumnType("float");

                entity.Property(e => e.Procent).HasColumnType("real");

                entity.Property(e => e.Bonus).HasColumnType("real");
            });

            modelBuilder.Entity<BuyRawMaterial>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.BuyRawMaterial)
                    .HasForeignKey(d => d.Unites)
                    .HasConstraintName("FK_BuyRawMaterial_Units");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Adress).HasMaxLength(50);

                entity.Property(e => e.Fio)
                    .HasColumnName("FIO")
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("Phone_Number")
                    .HasMaxLength(50);

                entity.HasOne(d => d.PositionNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Position)
                    .HasConstraintName("FK_Employees_Positions");
            });

            modelBuilder.Entity<FinishedProducts>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.FinishedProducts)
                    .HasForeignKey(d => d.Unites)
                    .HasConstraintName("FK_FinishedProducts_Units");
            });

            modelBuilder.Entity<GotovayaProduction>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Gotovaya_Production");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Unit)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Ingredients>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RawMaterial).HasColumnName("Raw_Material");

                entity.HasOne(d => d.ProductNavigation)
                    .WithMany(p => p.Ingredients)
                    .HasForeignKey(d => d.Product)
                    .HasConstraintName("FK_Ingredients_FinishedProducts");

                entity.HasOne(d => d.RawMaterialNavigation)
                    .WithMany(p => p.Ingredients)
                    .HasForeignKey(d => d.RawMaterial)
                    .HasConstraintName("FK_Ingredients_BuyRawMaterial");
            });

            modelBuilder.Entity<Positions>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Position).HasMaxLength(50);
            });

            modelBuilder.Entity<Produciya>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Produciya");

                entity.Property(e => e.Date).HasColumnType("smalldatetime");

                entity.Property(e => e.Fio)
                    .HasColumnName("FIO")
                    .HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Prodution>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("smalldatetime");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.Prodution)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK_Prodution_Employees");

                entity.HasOne(d => d.ProductNavigation)
                    .WithMany(p => p.Prodution)
                    .HasForeignKey(d => d.Product)
                    .HasConstraintName("FK_Prodution_FinishedProducts");
            });

            modelBuilder.Entity<PurchaseOfRawMaterials>(entity =>
            {
                entity.ToTable("Purchase_Of_Raw_Materials");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("smalldatetime");

                entity.Property(e => e.RawMaterial).HasColumnName("Raw_Material");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.PurchaseOfRawMaterials)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK_Purchase_Of_Raw_Materials_Employees");

                entity.HasOne(d => d.RawMaterialNavigation)
                    .WithMany(p => p.PurchaseOfRawMaterials)
                    .HasForeignKey(d => d.RawMaterial)
                    .HasConstraintName("FK_Purchase_Of_Raw_Materials_BuyRawMaterial");
            });

            modelBuilder.Entity<SaleOfProducts>(entity =>
            {
                entity.ToTable("Sale_Of_Products");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("smalldatetime");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.SaleOfProducts)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK_Sale_Of_Products_Employees");

                entity.HasOne(d => d.ProductNavigation)
                    .WithMany(p => p.SaleOfProducts)
                    .HasForeignKey(d => d.Product)
                    .HasConstraintName("FK_Sale_Of_Products_FinishedProducts");
            });

            modelBuilder.Entity<Units>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Unit)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Salaryes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Year_NameNavigation)
                    .WithMany(p => p.Salari)
                    .HasForeignKey(d => d.Year)
                    .HasConstraintName("FK_Salaryes_Years");

                entity.HasOne(d => d.MonthNavigation)
                    .WithMany(p => p.Salaryes)
                    .HasForeignKey(d => d.Month)
                    .HasConstraintName("FK_Salaryes_Months");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.Salari)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK_Salaryes_Employees");

                entity.Property(e => e.ParticipationInThePurchase).HasColumnType("int");

                entity.Property(e => e.ParticipationOnSale).HasColumnType("int");

                entity.Property(e => e.ParticipationOnProduction).HasColumnType("int");

                entity.Property(e => e.TotalNumberOfParticipations).HasComputedColumnSql("[ParticipationInThePurchase]+[ParticipationOnSale]+[ParticipationOnProduction]");

                entity.Property(e => e.Bonus).HasColumnType("real");

                entity.Property(e => e.Salary).HasColumnType("int");

                entity.Property(e => e.TotalSum).HasComputedColumnSql("[Bonus]+[Salary]");

                entity.Property(e => e.Boolean).HasColumnType("bit");
            });

            modelBuilder.Entity<Years>(entity =>
            {
                entity.Property(e => e.Year_Name).HasColumnName("Year_Name");
            });

            modelBuilder.Entity<Months>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Month_Name)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
