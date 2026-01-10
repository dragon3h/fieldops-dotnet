using Microsoft.AspNetCore.Mvc;
using FieldOps.Domain.BouncyCastle;
using FieldOps.Application.Interfaces.IServices;
using FieldOps.Application.DTOs;
using AutoMapper;

namespace FieldOps.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BouncyCastleController(IBouncyCastleService bouncyCastleService, IMapper mapper) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IBouncyCastleService _bouncyCastleService = bouncyCastleService;

    // GET: api/v1/bouncycastle
    [HttpGet]
    public async Task<ActionResult<List<BouncyCastleDTO>>> GetAll()
    {
        var castleList = await _bouncyCastleService.GetAllAsync() ?? new List<BouncyCastle>();

        var castleListResponse = _mapper.Map<List<BouncyCastleDTO>>(castleList);
        return Ok(castleListResponse);
    }

    // GET: api/v1/bouncycastle/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BouncyCastleDTO>> GetById(Guid id)
    {
        var castle = await _bouncyCastleService.GetByIdAsync(id);

        if (castle == null)
        {
            return NotFound();
        }

        var castleRequest = _mapper.Map<BouncyCastleDTO>(castle);

        return Ok(castleRequest);
    }

    // POST: api/v1/bouncycastle
    [HttpPost]
    public async Task<ActionResult<BouncyCastleDTO>> Create(BouncyCastleDTO castle)
    {
        var castleEntity = _mapper.Map<BouncyCastle>(castle);

        var createdCastle = await _bouncyCastleService.CreateAsync(castleEntity);

        return CreatedAtAction(nameof(GetById), new { id = createdCastle.Id },
            _mapper.Map<BouncyCastleDTO>(createdCastle));
    }

    // PUT: api/v1/bouncycastle/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, BouncyCastleDTO castle)
    {
        var existingCastle = await _bouncyCastleService.GetByIdAsync(id);
        if (id != castle.Id)
        {
            return BadRequest();
        }

        if (existingCastle == null)
        {
            return NotFound();
        }

        var mappedCastle = _mapper.Map(castle, existingCastle);
        await _bouncyCastleService.UpdateAsync(mappedCastle);

        return NoContent();
    }

    // DELETE: api/v1/bouncycastle/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var castle = await _bouncyCastleService.GetByIdAsync(id);

        if (castle == null)
        {
            return NotFound();
        }

        await _bouncyCastleService.DeleteAsync(castle);

        return NoContent();
    }
}