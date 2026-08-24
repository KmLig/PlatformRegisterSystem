using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Mappers;
using PlatformService.Models;

namespace PlatformService.Controllers;

[ApiController]
[Route("platforms")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repo;
    private readonly PlatformMappers _mapper;

    public PlatformsController(IPlatformRepo repo, PlatformMappers mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        IEnumerable<Platform> platforms = _repo.GetAllPlatforms();
        return Ok(platforms.Select(_mapper.MapToReadDto));
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        Platform? platform = _repo.GetPlatformById(id);
        return platform != null ? Ok(_mapper.MapToReadDto(platform)) : NotFound();
    }

    [HttpPost]
    public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        Platform platform = _mapper.MapToModel(platformCreateDto);
        _repo.CreatePlatform(platform);
        _repo.SaveChanges();

        PlatformReadDto platformReadDto = _mapper.MapToReadDto(platform);
        return CreatedAtRoute(nameof(GetPlatformById), new { id = platformReadDto.Id }, platformReadDto);
    }
}
