using Microsoft.AspNetCore.Mvc;
using FieldOps.Domain.BouncyCastle;
using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Application.Interfaces.IServices;
using FieldOps.Application.DTOs;
using AutoMapper;

namespace FieldOps.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BouncyCastleController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IBouncyCastleService _bouncyCastleService;

    public BouncyCastleController(IBouncyCastleService bouncyCastleService,
        IMapper mapper)
    {
        _bouncyCastleService = bouncyCastleService;
        _mapper = mapper;
    }

    // GET: api/v1/bouncycastle
    [HttpGet]
    public async Task<ActionResult<List<BouncyCastleDTO>>> GetAll()
    {
        var castleList = await _bouncyCastleService.GetAllBouncyCastlesAsync();

        if (castleList == null)
        {
            return NotFound();
        }

        var castleListRequest = _mapper.Map<List<BouncyCastleDTO>>(castleList);
        return Ok(castleListRequest);
    }

    // GET: api/v1/bouncycastle/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BouncyCastleDTO>> GetById(Guid id)
    {
        var castle = await _bouncyCastleService.GetBouncyCastleByIdAsync(id);

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

        var createdCastle = await _bouncyCastleService.CreateBouncyCastleAsync(castleEntity);

        return CreatedAtAction(nameof(GetById), new { id = createdCastle.Id },
            _mapper.Map<BouncyCastleDTO>(createdCastle));
    }

    // PUT: api/v1/bouncycastle/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, BouncyCastleDTO castle)
    {
        var existingCastle = await _bouncyCastleService.GetBouncyCastleByIdAsync(id);

        if (existingCastle == null)
        {
            return NotFound();
        }

        var mappedCastle = _mapper.Map(castle, existingCastle);
        await _bouncyCastleService.UpdateBouncyCastleAsync(mappedCastle);

        return NoContent();
    }

    // DELETE: api/v1/bouncycastle/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var castle = await _bouncyCastleService.GetBouncyCastleByIdAsync(id);

        if (castle == null)
        {
            return NotFound();
        }

        await _bouncyCastleService.DeleteBouncyCastleAsync(castle);

        return NoContent();
    }
}