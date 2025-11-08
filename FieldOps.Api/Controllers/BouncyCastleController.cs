using Microsoft.AspNetCore.Mvc;
using FieldOps.Infrastructure;
using FieldOps.Domain.BouncyCastle;
using FieldOps.Application.Interfaces.IRepositories;

namespace FieldOps.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BouncyCastleController : ControllerBase
{
  private readonly IUnitOfWork _unitOfWork;

  public BouncyCastleController(IRepository<BouncyCastle> repository, IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  // GET: api/v1/bouncycastle
  [HttpGet]
  public async Task<ActionResult<IEnumerable<BouncyCastle>>> GetAll()
  {
    var castles = await _repository.GetAllAsync();
    return Ok(castles);
  }

  // GET: api/v1/bouncycastle/{id}
  [HttpGet("{id:guid}")]
  public async Task<ActionResult<BouncyCastle>> GetById(Guid id)
  {
    var castle = await _repository.GetById(id);

    if (castle == null)
    {
      return NotFound();
    }

    return Ok(castle);
  }

  // POST: api/v1/bouncycastle
  [HttpPost]
  public async Task<ActionResult<BouncyCastle>> Create(BouncyCastle castle)
  {
    castle.Id = Guid.NewGuid();
    castle.CreatedAt = DateTime.UtcNow;

    await _repository.Create(castle);

    return CreatedAtAction(nameof(GetById), new { id = castle.Id }, castle); // todo: check understanding
  }

  // PUT: api/v1/bouncycastle/{id}
  [HttpPut("{id:guid}")]
  public async Task<IActionResult> Update(Guid id, BouncyCastle castle)
  {
    var existingCastle = await _repository.GetById(id);

    if (existingCastle == null)
    {
      return NotFound();
    }

    existingCastle.Name = castle.Name;
    existingCastle.Capacity = castle.Capacity;
    existingCastle.IsAvailable = castle.IsAvailable;
    existingCastle.UpdatedAt = DateTime.UtcNow;

    await _repository.Update(existingCastle);

    return NoContent();
  }

  // DELETE: api/v1/bouncycastle/{id}
  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> Delete(Guid id)
  {
    var castle = await _repository.GetById(id);

    if (castle == null)
    {
      return NotFound();
    }

    await _repository.Delete(id);

    return NoContent();
  }
}
