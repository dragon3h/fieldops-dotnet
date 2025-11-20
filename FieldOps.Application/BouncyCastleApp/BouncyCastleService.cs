using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Application.Interfaces.IServices;
using FieldOps.Domain.BouncyCastle;

namespace FieldOps.Application.BouncyCastleApp;

public class BouncyCastleService(IUnitOfWork unitOfWork) : IBouncyCastleService
{
    public async Task<List<BouncyCastle>> GetAllBouncyCastlesAsync()
    {
        var bouncyCastles = await unitOfWork.BouncyCastleRepository.GetAllAsync();
        return bouncyCastles.ToList();
    }
    
    public async Task<BouncyCastle?> GetBouncyCastleByIdAsync(Guid id)
    {
        return await unitOfWork.BouncyCastleRepository.GetById(id);
    }
    
    public async Task<BouncyCastle> CreateBouncyCastleAsync(BouncyCastle castle)
    {
        castle.Id = Guid.NewGuid();
        castle.CreatedAt = DateTime.UtcNow;
        castle.CreatedBy = "system"; // TODO: replace with actual user
        await unitOfWork.BouncyCastleRepository.Create(castle);
        await unitOfWork.CompleteAsync();
        
        return castle;
    }
    
    // how to handle async void?
    public async void UpdateBouncyCastleAsync(BouncyCastle castle)
    {
        castle.UpdatedAt = DateTime.UtcNow;
        castle.UpdatedBy = "system"; // TODO: replace with actual user
        await unitOfWork.BouncyCastleRepository.Update(castle);
        await unitOfWork.CompleteAsync();
    }
    
    // how to handle async void?
    public async void DeleteBouncyCastleAsync(BouncyCastle castle)
    {
        await unitOfWork.BouncyCastleRepository.Delete(castle);
        await unitOfWork.CompleteAsync();
    }
}