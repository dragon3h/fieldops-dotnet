using FieldOps.Domain.Abstractions;
using FieldOps.Domain.Shared;

namespace FieldOps.Domain.BouncyCastle;

public class BouncyCastle : Product
{
  public int Capacity { get; set; }
  public int AgeFor { get; set; }
  public BouncyCastleType BouncyCastleType { get; set; }
  public bool IsSplitUnit { get; set; }
  public Size? Size { get; set; }
  public int InflationTimeMinutes { get; set; }
  public bool PumpIncluded { get; set; }
}
