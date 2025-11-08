using FieldOps.Domain.Abstractions;
using FieldOps.Domain.Shared;

namespace FieldOps.Domain.Client;

public sealed class Client : BaseEntity
{
  public required string FirstName { get; set; }
  public string LastName { get; set; } = string.Empty;
  public string Email { get; set; }  = string.Empty;
  public string Description { get; set; }  = string.Empty;
  public required string ContactName { get; set; }
  public required string ContactPhone { get; set; }
  public string ContactEmail { get; set; } = string.Empty;
  public Address? Address { get; set; }
  public List<Order.Order> Orders { get; set; } = [];
  public List<Payment.Payment> Payments { get; set; } = [];
}