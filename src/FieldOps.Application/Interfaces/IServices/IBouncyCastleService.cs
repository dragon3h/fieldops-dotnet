using FieldOps.Domain.BouncyCastle;

namespace FieldOps.Application.Interfaces.IServices;

public interface IBouncyCastleService
{
    Task<List<BouncyCastle>> GetAllBouncyCastlesAsync();
    Task<BouncyCastle?> GetBouncyCastleByIdAsync(Guid id);
    Task<BouncyCastle> CreateBouncyCastleAsync(BouncyCastle castle);
    Task UpdateBouncyCastleAsync(BouncyCastle castle);
    Task DeleteBouncyCastleAsync(BouncyCastle castle);
}