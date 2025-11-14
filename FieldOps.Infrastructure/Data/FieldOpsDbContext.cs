using FieldOps.Domain.Abstractions;
using FieldOps.Domain.Order;
using Microsoft.EntityFrameworkCore;
using BouncyCastleEntity = FieldOps.Domain.BouncyCastle.BouncyCastle;
using ClientEntity = FieldOps.Domain.Client.Client;
using OrderEntity = FieldOps.Domain.Order.Order;
using PaymentEntity = FieldOps.Domain.Payment.Payment;

namespace FieldOps.Infrastructure.Data;

// todo: is one context for one DB ok?
public class FieldOpsDbContext : DbContext
{
  public FieldOpsDbContext(DbContextOptions<FieldOpsDbContext> options) : base(options)
  {
  }

  public DbSet<BouncyCastleEntity> BouncyCastles { get; set; }
  public DbSet<ClientEntity> Clients { get; set; }
  public DbSet<OrderEntity> Orders { get; set; }
  public DbSet<OrderItem> OrderItems { get; set; }
  public DbSet<PaymentEntity> Payments { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Configure TPC (Table Per Concrete Type) for Product hierarchy
    modelBuilder.Entity<Product>(entity =>
    {
      entity.UseTpcMappingStrategy();
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
      entity.Property(e => e.Description).HasMaxLength(1000);
      entity.Property(e => e.PurchasePrice).HasPrecision(18, 2);
      entity.Property(e => e.ResalePrice).HasPrecision(18, 2);
      entity.Property(e => e.RentalPrice).HasPrecision(18, 2);
      entity.Property(e => e.DepositPrice).HasPrecision(18, 2);
    });

    // BouncyCastle-specific configuration
    modelBuilder.Entity<BouncyCastleEntity>(entity =>
    {
      entity.ToTable("BouncyCastles");
      
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id)
        .ValueGeneratedNever();

      // Configure Size as owned entity (embedded in BouncyCastles table)
      entity.OwnsOne(e => e.Size, size =>
      {
        size.Property(s => s.LengthMeters).HasColumnName("Size_LengthMeters");
        size.Property(s => s.WidthMeters).HasColumnName("Size_WidthMeters");
        size.Property(s => s.HeightMeters).HasColumnName("Size_HeightMeters");
        size.Property(s => s.WeightKg).HasColumnName("Size_WeightKg");
      });
    });

    // Client configuration
    modelBuilder.Entity<ClientEntity>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id)
        .ValueGeneratedNever();
      entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
      entity.Property(e => e.LastName).HasMaxLength(100);
      entity.Property(e => e.Description).HasMaxLength(1000);
      entity.Property(e => e.ContactName).IsRequired().HasMaxLength(200);
      entity.Property(e => e.ContactPhone).IsRequired().HasMaxLength(20);
      entity.Property(e => e.ContactEmail).HasMaxLength(200);

      // Configure Address as owned entity (embedded in Clients table)
      // Address is optional - all properties are nullable, so we need to configure it properly
      entity.OwnsOne(e => e.Address, address =>
      {
        address.Property(a => a.AddressLine1).HasMaxLength(200).HasColumnName("Address_AddressLine1");
        address.Property(a => a.AddressLine2).HasMaxLength(200).HasColumnName("Address_AddressLine2");
        address.Property(a => a.City).HasMaxLength(100).HasColumnName("Address_City");
        address.Property(a => a.State).HasMaxLength(100).HasColumnName("Address_State");
        address.Property(a => a.ZipCode).HasMaxLength(20).HasColumnName("Address_ZipCode");
        address.Property(a => a.Country).HasMaxLength(100).HasColumnName("Address_Country");
      });

      entity.HasMany(e => e.Orders)
        .WithOne(o => o.Client)
        .HasForeignKey(o => o.ClientId)
        .OnDelete(DeleteBehavior.Restrict);

      entity.HasMany(e => e.Payments)
        .WithOne(p => p.Client)
        .HasForeignKey(p => p.ClientId)
        .OnDelete(DeleteBehavior.Restrict);
    });

    // Order configuration
    modelBuilder.Entity<OrderEntity>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id)
        .ValueGeneratedNever();
      entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
      entity.Property(e => e.EventLocation).HasMaxLength(500);
      entity.Property(e => e.Notes).HasMaxLength(2000);
      entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
      entity.Property(e => e.DepositAmount).HasPrecision(18, 2);

      entity.HasMany(e => e.OrderItems)
        .WithOne(oi => oi.Order)
        .HasForeignKey(oi => oi.OrderId)
        .OnDelete(DeleteBehavior.Cascade);

      entity.HasMany(e => e.Payments)
        .WithOne(p => p.Order)
        .HasForeignKey(p => p.OrderId)
        .OnDelete(DeleteBehavior.Restrict);
    });

    // OrderItem configuration
    modelBuilder.Entity<OrderItem>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id)
        .ValueGeneratedNever();
      entity.Property(e => e.RentalPricePerUnit).HasPrecision(18, 2);
      entity.Property(e => e.DepositPricePerUnit).HasPrecision(18, 2);

      // Ignore computed properties (not stored in database)
      entity.Ignore(e => e.LineTotal);
      entity.Ignore(e => e.LineTotalDeposit);
    });

    // Payment configuration
    modelBuilder.Entity<PaymentEntity>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Id)
        .ValueGeneratedNever();
      entity.Property(e => e.Amount).HasPrecision(18, 2);
      entity.Property(e => e.PaymentMethod).HasMaxLength(50);
      entity.Property(e => e.TransactionId).HasMaxLength(200);
    });
  }
}
