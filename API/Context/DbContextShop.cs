using System;
using System.Collections.Generic;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Context;

public partial class DbContextShop : DbContext
{
    public DbContextShop()
    {
    }

    public DbContextShop(DbContextOptions<DbContextShop> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<BillDetail> BillDetails { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductDetail> ProductDetails { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Staff> Staffs { get; set; }

    public virtual DbSet<Transport> Transports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DEVMANH;Initial Catalog=Duantotnghiep;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasOne(d => d.Customer).WithMany(p => p.Bills).HasConstraintName("FK_Bills_Customers");

            entity.HasOne(d => d.Order).WithMany(p => p.Bills).HasConstraintName("FK_Bills_Order");

            entity.HasOne(d => d.Staff).WithMany(p => p.Bills).HasConstraintName("FK_Bills_Staffs");

            entity.HasOne(d => d.Transport).WithMany(p => p.Bills).HasConstraintName("FK_Bills_Transport");
        });

        modelBuilder.Entity<BillDetail>(entity =>
        {
            entity.HasOne(d => d.Bill).WithMany(p => p.BillDetails).HasConstraintName("FK_BillDetails_Bills");

            entity.HasOne(d => d.Product).WithMany(p => p.BillDetails).HasConstraintName("FK_BillDetails_Products");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.ColorId).HasName("PK__Colors__8DA7676D341FD042");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK__Material__C506131791209DD9");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(d => d.Customer).WithMany(p => p.Orders).HasConstraintName("FK_Order_Customers");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6EDADF9CF27");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK__Products__Catego__3E52440B");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__ProductC__19093A2B56D341DA");
        });

        modelBuilder.Entity<ProductDetail>(entity =>
        {
            entity.HasKey(e => e.ProductDetailId).HasName("PK__ProductD__135C314DBA604747");

            entity.HasOne(d => d.Color).WithMany(p => p.ProductDetails).HasConstraintName("FK__ProductDe__Color__4AB81AF0");

            entity.HasOne(d => d.Material).WithMany(p => p.ProductDetails).HasConstraintName("FK__ProductDe__Mater__4CA06362");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductDetails).HasConstraintName("FK__ProductDe__Produ__49C3F6B7");

            entity.HasOne(d => d.Size).WithMany(p => p.ProductDetails).HasConstraintName("FK__ProductDe__SizeI__4BAC3F29");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PK__Sizes__83BD095A5DB06CC4");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
