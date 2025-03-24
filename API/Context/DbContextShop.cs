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

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<BillDetail> BillDetails { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductDetail> ProductDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Staff> Staffs { get; set; }

    public virtual DbSet<Transport> Transports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.

        => optionsBuilder.UseSqlServer("Data Source=DEVMANH;Initial Catalog=Duantotnghiep;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5866CC4CC0D");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Accounts__RoleID__123EB7A3");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__Bills__11F2FC4A635723A7");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.Customer).WithMany(p => p.Bills)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Bills__CustomerI__619B8048");

            entity.HasOne(d => d.Staff).WithMany(p => p.Bills)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Bills__StaffID__60A75C0F");

            entity.HasOne(d => d.Transport).WithMany(p => p.Bills)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Bills__Transport__628FA481");
        });

        modelBuilder.Entity<BillDetail>(entity =>
        {
            entity.HasKey(e => e.BillDetailId).HasName("PK__BillDeta__793CAF7565BBFBEC");

            entity.Property(e => e.Total).HasComputedColumnSql("([Quantity]*[UnitPrice])", true);

            entity.HasOne(d => d.Bill).WithMany(p => p.BillDetails)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__BillDetai__BillI__6E01572D");

            entity.HasOne(d => d.ProductDetail).WithMany(p => p.BillDetails)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__BillDetai__Produ__6EF57B66");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.ColorId).HasName("PK__Colors__8DA7676D46814297");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B893D88A52");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Point).HasDefaultValue(0);
            entity.Property(e => e.RankMember).HasDefaultValue("Bình thường");
            entity.Property(e => e.RoleId).HasDefaultValue(3);

            entity.HasOne(d => d.Role).WithMany(p => p.Customers).HasConstraintName("FK_Customers_Roles");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK__Material__C5061317129D5857");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6EDC32A859E");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Products__Catego__68487DD7");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__ProductC__19093A2BB1696ED5");
        });

        modelBuilder.Entity<ProductDetail>(entity =>
        {
            entity.HasKey(e => e.ProductDetailId).HasName("PK__ProductD__3C8DD694357756D8");

            entity.HasOne(d => d.Color).WithMany(p => p.ProductDetails)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__ProductDe__Color__7E37BEF6");

            entity.HasOne(d => d.Material).WithMany(p => p.ProductDetails)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__ProductDe__Mater__7F2BE32F");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductDetails)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ProductDe__Produ__7C4F7684");

            entity.HasOne(d => d.Size).WithMany(p => p.ProductDetails)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__ProductDe__SizeI__7D439ABD");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3AD721524A");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PK__Sizes__83BD095A4656A287");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staffs__96D4AAF74B920184");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.RoleId).HasDefaultValue(2);
        });

        modelBuilder.Entity<Transport>(entity =>
        {
            entity.HasKey(e => e.TransportId).HasName("PK__Transpor__19E9A17DB524B435");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
