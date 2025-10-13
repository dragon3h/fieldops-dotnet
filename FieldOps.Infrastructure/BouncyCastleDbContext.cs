using Microsoft.EntityFrameworkCore;
using FieldOps.Infrastructure.Entities;

namespace FieldOps.Infrastructure;

public class BouncyCastleDbContext : DbContext
{
  public BouncyCastleDbContext(DbContextOptions<BouncyCastleDbContext> options) : base(options)
  {
  }

  public DbSet<BouncyCastle> BouncyCastles { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<BouncyCastle>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
      entity.Property(e => e.Location).HasMaxLength(500);
    });
  }
}
