using FieldOps.Domain.BouncyCastle;
using FieldOps.Domain.Shared;

namespace FieldOps.Infrastructure.DTOs;

public class BouncyCastleDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal ResalePrice { get; set; }
    public decimal RentalPrice { get; set; }
    public decimal DepositPrice { get; set; }
    public ProductType ProductType { get; set; }
    public int Capacity { get; set; }
    public int AgeFor { get; set; }
    public BouncyCastleType BouncyCastleType { get; set; }
    public bool IsSplitUnit { get; set; }
    public Size? Size { get; set; }
    public int InflationTimeMinutes { get; set; }
    public bool PumpIncluded { get; set; }
}