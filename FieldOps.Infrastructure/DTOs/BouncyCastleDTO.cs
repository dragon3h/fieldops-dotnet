using FieldOps.Domain.BouncyCastle;
using FieldOps.Domain.Shared;

namespace FieldOps.Infrastructure.DTOs;

public class BouncyCastleDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal ResalePrice { get; set; }
    public decimal RentalPrice { get; set; }
    public decimal DepositPrice { get; set; }
    public ProductType ProductType { get; set; }
    public int Capacity { get; set; } = default;
    public int AgeFor { get; set; } = default;
    public BouncyCastleType BouncyCastleType { get; set; }
    public bool IsSplitUnit { get; set; } = default;
    public Size? Size { get; set; }
    public int InflationTimeMinutes { get; set; } = default;
    public bool PumpIncluded { get; set; } = default;
}