﻿// <auto-generated />
using System;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataAccessLayer.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Items");

                    b.HasData(
                        new
                        {
                            Id = 14665,
                            CategoryName = "Electronics",
                            Description = "High-performance laptop with 16GB RAM and 512GB SSD.",
                            Name = "Laptop",
                            Quantity = 50
                        },
                        new
                        {
                            Id = 20312,
                            CategoryName = "Electronics",
                            Description = "Ergonomic wireless mouse with long battery life.",
                            Name = "Wireless Mouse",
                            Quantity = 200
                        },
                        new
                        {
                            Id = 22314,
                            CategoryName = "Furniture",
                            Description = "Adjustable office chair with lumbar support.",
                            Name = "Office Chair",
                            Quantity = 30
                        },
                        new
                        {
                            Id = 12314,
                            CategoryName = "Furniture",
                            Description = "Adjustable office chair with lumbar support.",
                            Name = "Office Chair",
                            Quantity = 30
                        },
                        new
                        {
                            Id = 42342,
                            CategoryName = "Kitchen Appliances",
                            Description = "High-speed blender for smoothies and soups.",
                            Name = "Blender",
                            Quantity = 75
                        },
                        new
                        {
                            Id = 52331,
                            CategoryName = "Stationery",
                            Description = "Spiral-bound notebook with 200 pages.",
                            Name = "Notebook",
                            Quantity = 1000
                        },
                        new
                        {
                            Id = 51234,
                            CategoryName = "Electronics",
                            Description = "Latest model smartphone with 5G support and 128GB storage.",
                            Name = "Smartphone",
                            Quantity = 40
                        });
                });

            modelBuilder.Entity("DataAccessLayer.Models.ItemLedger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemLedgers");
                });

            modelBuilder.Entity("DataAccessLayer.Models.PurchaseOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1000L);

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SupplierId");

                    b.ToTable("PurchaseOrders");
                });

            modelBuilder.Entity("DataAccessLayer.Models.PurchaseOrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("PurchaseOrderId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("PurchaseOrderId");

                    b.ToTable("PurchaseOrderItems");
                });

            modelBuilder.Entity("DataAccessLayer.Models.PurchaseReceipt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1000L);

                    b.Property<int>("PurchaseOrderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReceiptDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseOrderId");

                    b.ToTable("PurchaseReceipts");
                });

            modelBuilder.Entity("DataAccessLayer.Models.PurchaseReceiptItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("PurchaseReceiptId")
                        .HasColumnType("int");

                    b.Property<int>("ReceivedQuantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("PurchaseReceiptId");

                    b.ToTable("PurchaseReceiptItems");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 100L);

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("Suppliers");

                    b.HasData(
                        new
                        {
                            Id = 100,
                            Address = "Hamra Street, Beirut, Lebanon",
                            Email = "contact@beirutsupplies.com",
                            Name = "Beirut Supplies Co.",
                            PhoneNumber = "+961 1 234 567"
                        },
                        new
                        {
                            Id = 101,
                            Address = "Zalka, Metn, Lebanon",
                            Email = "info@lebanesefoods.com",
                            Name = "Lebanese Food Services",
                            PhoneNumber = "+961 3 876 543"
                        },
                        new
                        {
                            Id = 102,
                            Address = "Downtown, Beirut, Lebanon",
                            Email = "sales@cedars-import.com",
                            Name = "Cedars Import & Export",
                            PhoneNumber = "+961 1 998 877"
                        });
                });

            modelBuilder.Entity("DataAccessLayer.Models.ItemLedger", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("DataAccessLayer.Models.PurchaseOrder", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("DataAccessLayer.Models.PurchaseOrderItem", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Models.PurchaseOrder", "PurchaseOrder")
                        .WithMany("OrderItems")
                        .HasForeignKey("PurchaseOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("PurchaseOrder");
                });

            modelBuilder.Entity("DataAccessLayer.Models.PurchaseReceipt", b =>
                {
                    b.HasOne("DataAccessLayer.Models.PurchaseOrder", "PurchaseOrder")
                        .WithMany()
                        .HasForeignKey("PurchaseOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PurchaseOrder");
                });

            modelBuilder.Entity("DataAccessLayer.Models.PurchaseReceiptItem", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Models.PurchaseReceipt", "PurchaseReceipt")
                        .WithMany("ReceiptItems")
                        .HasForeignKey("PurchaseReceiptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("PurchaseReceipt");
                });

            modelBuilder.Entity("DataAccessLayer.Models.PurchaseOrder", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("DataAccessLayer.Models.PurchaseReceipt", b =>
                {
                    b.Navigation("ReceiptItems");
                });
#pragma warning restore 612, 618
        }
    }
}
