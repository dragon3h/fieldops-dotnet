using Microsoft.AspNetCore.Mvc;
using FieldOps.Infrastructure;
using FieldOps.Infrastructure.Entities;

namespace FieldOps.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BouncyCastleController : ControllerBase
{
  private readonly IRepository<BouncyCastle> _repository;

  public BouncyCastleController(IRepository<BouncyCastle> repository)
  {
    _repository = repository;
  }

  // GET: api/v1/bouncycastle
  [HttpGet]
  public async Task<ActionResult<IEnumerable<BouncyCastle>>> GetAll()
  {
    var castles = await _repository.GetAllAsync();
    return Ok(castles);
  }

  // GET: api/v1/bouncycastle/5
  [HttpGet("{id}")]
  public async Task<ActionResult<BouncyCastle>> GetById(int id)
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
  public async Task<ActionResult<BouncyCastle>> Create([FromBody] BouncyCastle castle)
  {
    castle.CreatedAt = DateTime.UtcNow;

    await _repository.Create(castle);

    return CreatedAtAction(nameof(GetById), new { id = castle.Id }, castle);
  }

  // PUT: api/v1/bouncycastle/5
  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, [FromBody] BouncyCastle castle)
  {
    var existingCastle = await _repository.GetById(id);

    if (existingCastle == null)
    {
      return NotFound();
    }

    existingCastle.Name = castle.Name;
    existingCastle.Location = castle.Location;
    existingCastle.Capacity = castle.Capacity;
    existingCastle.IsAvailable = castle.IsAvailable;
    existingCastle.UpdatedAt = DateTime.UtcNow;

    await _repository.Update(existingCastle);

    return NoContent();
  }

  // DELETE: api/v1/bouncycastle/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
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
