using AlumniNetwork.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlumniNetwork.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController(IAdminService adminService, IPostService postService) : ControllerBase
{
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await adminService.GetAllUsersAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("users/{userId:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int userId, CancellationToken cancellationToken)
    {
        var result = await adminService.DeleteUserAsync(userId, cancellationToken);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpDelete("posts/{postId:int}")]
    public async Task<IActionResult> ModeratePost([FromRoute] int postId, CancellationToken cancellationToken)
    {
        var result = await postService.ModerateDeletePostAsync(postId, cancellationToken);
        return result.Success ? Ok(result) : NotFound(result);
    }
}
