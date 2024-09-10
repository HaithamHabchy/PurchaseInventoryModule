using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<PurchaseReceipt> PurchaseReceipts { get; set; }
        public DbSet<PurchaseReceiptItem> PurchaseReceiptItems { get; set; }
        public DbSet<ItemLedger> ItemLedgers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Generates PurchaseOrder IDs starting from 1000.
            modelBuilder.Entity<PurchaseOrder>()
            .Property(p => p.Id)
            .UseIdentityColumn(1000, 1);

            // Generates PurchaseReceipt IDs starting from 1000.
            modelBuilder.Entity<PurchaseReceipt>()
            .Property(p => p.Id)
            .UseIdentityColumn(1000, 1);

            // Generates Supplier IDs starting from 1000.
            modelBuilder.Entity<Supplier>()
            .Property(p => p.Id)
            .UseIdentityColumn(100, 1);

            // Specify precision and scale for decimal properties
            modelBuilder.Entity<PurchaseOrder>()
                .Property(p => p.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(p => p.UnitPrice)
                .HasColumnType("decimal(18,2)");

            // Add index on Email and PhoneNumber
            modelBuilder.Entity<Supplier>()
                .HasIndex(s => s.Email)
                .IsUnique();

            modelBuilder.Entity<Supplier>()
                .HasIndex(s => s.PhoneNumber)
                .IsUnique();

            // Seeding initial supplier data into the database.
            modelBuilder.Entity<Supplier>().HasData(
              new Supplier
              {
                  Id = 100,
                  Name = "Beirut Supplies Co.",
                  Email = "contact@beirutsupplies.com",
                  PhoneNumber = "+961 1 234 567",
                  Address = "Hamra Street, Beirut, Lebanon"
              },
              new Supplier
              {
                  Id = 101,
                  Name = "Lebanese Food Services",
                  Email = "info@lebanesefoods.com",
                  PhoneNumber = "+961 3 876 543",
                  Address = "Zalka, Metn, Lebanon"
              },
              new Supplier
              {
                  Id = 102,
                  Name = "Cedars Import & Export",
                  Email = "sales@cedars-import.com",
                  PhoneNumber = "+961 1 998 877",
                  Address = "Downtown, Beirut, Lebanon"
              });

            // Seeding initial Item data into the database.
            modelBuilder.Entity<Item>().HasData(
              new Item
              {
                  Id = 14665,
                  Name = "Laptop",
                  Quantity = 50,
                  Description = "High-performance laptop with 16GB RAM and 512GB SSD.",
                  CategoryName = "Electronics"
              },
              new Item
              {
                  Id = 20312,
                  Name = "Wireless Mouse",
                  Quantity = 200,
                  Description = "Ergonomic wireless mouse with long battery life.",
                  CategoryName = "Electronics"
              },
              new Item
              {
                  Id = 22314,
                  Name = "Office Chair",
                  Quantity = 30,
                  Description = "Adjustable office chair with lumbar support.",
                  CategoryName = "Furniture"
              },
              new Item
              {
                  Id = 12314,
                  Name = "Office Chair",
                  Quantity = 30,
                  Description = "Adjustable office chair with lumbar support.",
                  CategoryName = "Furniture"
              },
              new Item
              {
                  Id = 42342,
                  Name = "Blender",
                  Quantity = 75,
                  Description = "High-speed blender for smoothies and soups.",
                  CategoryName = "Kitchen Appliances"
              },
              new Item
              {
                  Id = 52331,
                  Name = "Notebook",
                  Quantity = 1000,
                  Description = "Spiral-bound notebook with 200 pages.",
                  CategoryName = "Stationery"
              },
              new Item
              {
                  Id = 51234,
                  Name = "Smartphone",
                  Quantity = 40,
                  Description = "Latest model smartphone with 5G support and 128GB storage.",
                  CategoryName = "Electronics"
              });
        }
    }
}
