using FieldOps.Domain.Orders;
using FieldOps.Domain.Payment;
using FieldOps.Domain.Shared;

namespace FieldOps.Application.DTOs;

public class ClientDTO
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public required string ContactName { get; set; }
    public required string ContactPhone { get; set; }
    public string ContactEmail { get; set; }
    public Address? Address { get; set; }

    // todo: when OrderDTO is created, change List<Order> to List<OrderDTO>
    public List<Order> Orders { get; set; } = [];
    // todo: when PaymentDTO is created, change List<Payment> to List<PaymentDTO>
    public List<Payment> Payments { get; set; } = [];
}