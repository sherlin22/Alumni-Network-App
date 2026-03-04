using AlumniNetwork.Application.DTOs;
using AlumniNetwork.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AlumniNetwork.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostsController(IPostService postService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequestDto request, CancellationToken cancellationToken)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);
        var result = await postService.CreatePostAsync(userId, request, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetPosts([FromQuery] string? department, [FromQuery] int? year, [FromQuery] string? location, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new PostQueryDto(department, year, location, pageNumber, pageSize);
        var result = await postService.GetPostsAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{postId:int}/like")]
    public async Task<IActionResult> LikePost([FromRoute] int postId, CancellationToken cancellationToken)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);
        var result = await postService.LikePostAsync(userId, postId, cancellationToken);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost("{postId:int}/comment")]
    public async Task<IActionResult> AddComment([FromRoute] int postId, [FromBody] AddCommentRequestDto request, CancellationToken cancellationToken)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);
        var result = await postService.AddCommentAsync(userId, postId, request, cancellationToken);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [AllowAnonymous]
    [HttpGet("{postId:int}/comments")]
    public async Task<IActionResult> GetComments([FromRoute] int postId, CancellationToken cancellationToken)
    {
        var result = await postService.GetCommentsAsync(postId, cancellationToken);
        return Ok(result);
    }
}
