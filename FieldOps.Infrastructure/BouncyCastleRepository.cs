using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using FieldOps.Infrastructure.Entities;

namespace FieldOps.Infrastructure;

public class BouncyCastleRepository : IRepository<BouncyCastle>, IDisposable
{

  private readonly BouncyCastleDbContext _context;
  public BouncyCastleRepository(BouncyCastleDbContext context)
  {
    _context = context;
  }

  public async Task<List<BouncyCastle>> GetAllAsync()
  {
    return await _context.BouncyCastles.ToListAsync();
  }

  public async Task<BouncyCastle?> GetById(int id)
  {
    return await _context.BouncyCastles.FindAsync(id);
  }

  public async Task Create(BouncyCastle entity)
  {
    _context.BouncyCastles.Add(entity);
    await _context.SaveChangesAsync();
  }

  public async Task Update(BouncyCastle entity)
  {
    _context.BouncyCastles.Update(entity);
    await _context.SaveChangesAsync();
  }

  public async Task Delete(int id)
  {
    var entity = await _context.BouncyCastles.FindAsync(id);
    if (entity != null)
    {
      _context.BouncyCastles.Remove(entity);
      await _context.SaveChangesAsync();
    }
  }
}