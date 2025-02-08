using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Test.Entities;

public partial class SMSanagement_DBContext : DbContext
{
    public SMSanagement_DBContext()
    {
    }

    public SMSanagement_DBContext(DbContextOptions<SMSanagement_DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AssetInformation> AssetInformation { get; set; }

    public virtual DbSet<BookingInformation> BookingInformation { get; set; }

    public virtual DbSet<Customer> Customer { get; set; }

    public virtual DbSet<PrinterSelection> PrinterSelection { get; set; }

    public virtual DbSet<Product> Product { get; set; }

    public virtual DbSet<ProductInformation> ProductInformation { get; set; }

    public virtual DbSet<Sale> Sale { get; set; }

    public virtual DbSet<Sales_FollowUpDealingInformation> Sales_FollowUpDealingInformation { get; set; }

    public virtual DbSet<Tbl_HRM_CustomarInformation> Tbl_HRM_CustomarInformation { get; set; }

    public virtual DbSet<Tbl_HRM_EmployeeInformation> Tbl_HRM_EmployeeInformation { get; set; }

    public virtual DbSet<Tbl_HRM_UserInformation> Tbl_HRM_UserInformation { get; set; }

    public virtual DbSet<UserTokens> UserTokens { get; set; }

    public virtual DbSet<View_Cricket_CrecketerInformation> View_Cricket_CrecketerInformation { get; set; }

    public virtual DbSet<View_SMS_PendingMessage> View_SMS_PendingMessage { get; set; }

    public virtual DbSet<WarrantyInformation> WarrantyInformation { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=SPLITPC003\\SQL19;Database=SMSanagement_DB;User Id=test;Password=123456789;TrustServerCertificate=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssetInformation>(entity =>
        {
            entity.HasKey(e => e.AssetId);

            entity.HasIndex(e => e.ItemId, "UK_AssetInformation_ItemId").IsUnique();

            entity.Property(e => e.ItemDescription).HasMaxLength(250);
            entity.Property(e => e.ItemId).HasMaxLength(150);
            entity.Property(e => e.ItemStatus).HasMaxLength(50);
            entity.Property(e => e.UserEID).HasMaxLength(10);
        });

        modelBuilder.Entity<BookingInformation>(entity =>
        {
            entity.HasKey(e => e.ParcelId);

            entity.Property(e => e.BookingBranchBng).HasMaxLength(50);
            entity.Property(e => e.BookingBranchEng).HasMaxLength(50);
            entity.Property(e => e.BookingType).HasMaxLength(50);
            entity.Property(e => e.CNNO).HasMaxLength(50);
            entity.Property(e => e.HV).HasMaxLength(50);
            entity.Property(e => e.ItemDetails).HasMaxLength(50);
            entity.Property(e => e.ParcelType).HasMaxLength(50);
            entity.Property(e => e.ReceiveBranchBng).HasMaxLength(50);
            entity.Property(e => e.ReceiveBranchEng).HasMaxLength(50);
            entity.Property(e => e.ReceiverAddress).HasMaxLength(50);
            entity.Property(e => e.ReceiverContact).HasMaxLength(50);
            entity.Property(e => e.ReceiverName).HasMaxLength(50);
            entity.Property(e => e.Referance).HasMaxLength(50);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Person");

            entity.ToTable(tb => tb.HasTrigger("tr_dbo_Customer_f721e9d3-d4cb-4af8-847b-4a7413dc1ecc"));

            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<PrinterSelection>(entity =>
        {
            entity.HasKey(e => e.PrinterId);

            entity.HasIndex(e => e.AccessUser, "UK_PrinterSelection_AccessUser").IsUnique();

            entity.Property(e => e.AccessUser).HasMaxLength(50);
            entity.Property(e => e.BarcodePrinter).HasMaxLength(50);
            entity.Property(e => e.CNPrinter).HasMaxLength(150);
            entity.Property(e => e.HV)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("H")
                .IsFixedLength();
            entity.Property(e => e.SelectedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable(tb =>
                {
                    tb.HasTrigger("tr_dbo_Product_03e044e1-7cb2-41e3-88b8-da54f7511e2a");
                    tb.HasTrigger("tr_dbo_Product_0b0f539b-8c20-437c-808d-a3621e7c9a1f");
                    tb.HasTrigger("tr_dbo_Product_ff3a6198-d3c1-424e-b776-2ce123826d72");
                });

            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasMaxLength(550);
            entity.Property(e => e.Image).HasMaxLength(550);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<ProductInformation>(entity =>
        {
            entity.HasKey(e => e.ProductId);

            entity.Property(e => e.Category).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Sales");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("tr_dbo_Sale_185cb40d-d675-43d1-abac-7dc3345b71f7");
                    tb.HasTrigger("tr_dbo_Sale_2882a185-9da5-4aff-a650-a25b2db56bcc");
                });

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Customer)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sales_FollowUpDealingInformation>(entity =>
        {
            entity.HasKey(e => e.FollowUpDealingId);

            entity.Property(e => e.ActualPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppSize).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ApprovedBy).HasMaxLength(150);
            entity.Property(e => e.Block).HasMaxLength(50);
            entity.Property(e => e.BookingDate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BookingMoney).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CarParking).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClientProposal).HasMaxLength(250);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DealingDescription).HasMaxLength(500);
            entity.Property(e => e.DealingOfficer).HasMaxLength(50);
            entity.Property(e => e.DownPayment)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.EditDate).HasColumnType("datetime");
            entity.Property(e => e.Facing).HasMaxLength(150);
            entity.Property(e => e.Floor).HasMaxLength(50);
            entity.Property(e => e.ManagementOrder).HasMaxLength(250);
            entity.Property(e => e.PaymentType).HasMaxLength(50);
            entity.Property(e => e.Plot).HasMaxLength(20);
            entity.Property(e => e.RateAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RateType).HasMaxLength(50);
            entity.Property(e => e.Remarks).HasMaxLength(150);
            entity.Property(e => e.Road).HasMaxLength(150);
            entity.Property(e => e.Sector).HasMaxLength(20);
            entity.Property(e => e.SpecialDiscount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.TotalArea).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Utility).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Tbl_HRM_CustomarInformation>(entity =>
        {
            entity.HasKey(e => e.CustomarId).HasName("PK_CustomarInformation");

            entity.HasIndex(e => e.CustomarId, "UK_Tbl_HRM_CustomarInformation_UserName").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(150);
            entity.Property(e => e.Mobile).HasMaxLength(150);
            entity.Property(e => e.UserName).HasMaxLength(150);
        });

        modelBuilder.Entity<Tbl_HRM_EmployeeInformation>(entity =>
        {
            entity.HasKey(e => e.EmpId);

            entity.Property(e => e.EmpAddress).HasMaxLength(50);
            entity.Property(e => e.EmpContact).HasMaxLength(50);
            entity.Property(e => e.EmpName).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<Tbl_HRM_UserInformation>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_UserInformation");

            entity.Property(e => e.LoginName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserEid).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(150);
        });

        modelBuilder.Entity<UserTokens>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__UserToke__1788CC4CA380186A");

            entity.Property(e => e.Expiration).HasColumnType("datetime");
            entity.Property(e => e.LoginName).HasMaxLength(50);
            entity.Property(e => e.RefreshToken).HasMaxLength(256);
        });

        modelBuilder.Entity<View_Cricket_CrecketerInformation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Cricket_CrecketerInformation");

            entity.Property(e => e.CricketId).ValueGeneratedOnAdd();
            entity.Property(e => e.CricketerAddress).HasMaxLength(500);
            entity.Property(e => e.CricketerMobile).HasMaxLength(50);
            entity.Property(e => e.CricketerName).HasMaxLength(50);
        });

        modelBuilder.Entity<View_SMS_PendingMessage>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_SMS_PendingMessage");

            entity.Property(e => e.LoginName).HasMaxLength(50);
            entity.Property(e => e.UserEid).HasMaxLength(50);
        });

        modelBuilder.Entity<WarrantyInformation>(entity =>
        {
            entity.HasKey(e => e.WarrantyId);

            entity.Property(e => e.ItemDescription).HasMaxLength(150);
            entity.Property(e => e.ItemId).HasMaxLength(50);
            entity.Property(e => e.UserEID).HasMaxLength(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
