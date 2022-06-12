using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ShoppingCartCMC.Server.Shared.DB.Trading
{
    public partial class ShoppingCartCmcTradingContext : DbContext
    {
        public ShoppingCartCmcTradingContext()
        {
        }

        public ShoppingCartCmcTradingContext(DbContextOptions<ShoppingCartCmcTradingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Billing> Billings { get; set; }
        public virtual DbSet<BillingProduct> BillingProducts { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=ShoppingCartCmc.Trading;Trusted_Connection=True;");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Billing>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.ToTable("Billing");

                entity.Property(e => e.Key)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Address1).HasMaxLength(500);

                entity.Property(e => e.Address2).HasMaxLength(500);

                entity.Property(e => e.Country).HasMaxLength(500);

                entity.Property(e => e.EmailId).HasMaxLength(250);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.ShippingFee).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.State).HasMaxLength(50);

                entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<BillingProduct>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BillingProduct");

                entity.Property(e => e.BillingKey)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.ProductKey)
                    .HasMaxLength(50)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.ToTable("Product");

                entity.Property(e => e.Key)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Category).HasMaxLength(250);

                entity.Property(e => e.Currency)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .IsFixedLength(true);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Rating).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Seller).HasMaxLength(250);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
