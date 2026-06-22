using PlatformService.Dtos;
using PlatformService.Models;
using Riok.Mapperly.Abstractions;

namespace PlatformService.Mappers;

[Mapper]
public partial class PlatformMappers
{
    // Mapperly will generate the implementation for this method
    public partial PlatformReadDto MapToReadDto(Platform platform);
}