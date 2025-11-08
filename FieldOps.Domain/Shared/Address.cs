namespace FieldOps.Domain.Shared;

public record Address(string? AddressLine1 = null, string? AddressLine2 = null, string? City = null, string? State = null, string? ZipCode = null, string? Country = null);