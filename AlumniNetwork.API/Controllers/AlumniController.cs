using AlumniNetwork.Application.DTOs;
using AlumniNetwork.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlumniNetwork.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlumniController(IUserService userService) : ControllerBase
{
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? department, [FromQuery] int? year, [FromQuery] string? location, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new AlumniSearchQueryDto(department, year, location, pageNumber, pageSize);
        var result = await userService.SearchAlumniAsync(query, cancellationToken);
        return Ok(result);
    }
}
