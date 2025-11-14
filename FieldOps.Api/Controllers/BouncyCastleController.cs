using Microsoft.AspNetCore.Mvc;
using FieldOps.Domain.BouncyCastle;
using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Infrastructure.DTOs;

namespace FieldOps.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BouncyCastleController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper<BouncyCastle, BouncyCastleDTO> _mapper;

    public BouncyCastleController(IUnitOfWork unitOfWork, IMapper<BouncyCastle, BouncyCastleDTO> mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // GET: api/v1/bouncycastle
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BouncyCastleDTO>>>
        GetAll() // what is better here: ActionResult<List<BouncyCastleDTO>> or ActionResult<BouncyCastleDTO> or IActionResult?
    {
        var castles = await _unitOfWork.BouncyCastleRepository.GetAllAsync();

        if (castles == null)
        {
            return NotFound();
        }

        var castleList = castles.ToList();
        var castleListRequest = _mapper.MapList(castleList);
        return Ok(castleListRequest);
    }

    // GET: api/v1/bouncycastle/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BouncyCastle>> GetById(Guid id)
    {
        var castle = await _unitOfWork.BouncyCastleRepository.GetById(id);

        if (castle == null)
        {
            return NotFound();
        }

        var castleRequest = _mapper.Map(castle);

        return Ok(castleRequest);
    }

    // POST: api/v1/bouncycastle
    [HttpPost]
    public async Task<ActionResult<BouncyCastleDTO>> Create(BouncyCastleDTO castle)
    {
        castle.Id = Guid.NewGuid();
        var castleEntity = _mapper.MapForCreation(castle);
        castleEntity.CreatedAt = DateTime.UtcNow;
        await _unitOfWork.BouncyCastleRepository.Create(castleEntity);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetById), new { id = castleEntity.Id },
            _mapper.Map(castleEntity));
    }

    // PUT: api/v1/bouncycastle/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, BouncyCastleDTO castle)
    {
        var existingCastle = await _unitOfWork.BouncyCastleRepository.GetById(id);

        if (existingCastle == null)
        {
            return NotFound();
        }
        
        var mappedCastle = _mapper.MapForUpdate(existingCastle, castle);
        await _unitOfWork.BouncyCastleRepository.Update(mappedCastle);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    // DELETE: api/v1/bouncycastle/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var castle = await _unitOfWork.BouncyCastleRepository.GetById(id);

        if (castle == null)
        {
            return NotFound();
        }

        await _unitOfWork.BouncyCastleRepository.Delete(castle);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}