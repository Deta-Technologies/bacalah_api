using bacalah.API.Models;
using bacalah.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bacalah.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;
    
    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TagDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<List<TagDto>>), 500)]
    public async Task<ActionResult<ApiResponseDto<List<TagDto>>>> GetAllAsync()
    {
        try
        {
            var tags = await _tagService.GetAllAsync();
            return Ok(new ApiResponseDto<List<TagDto>>
            {
                Success = true,
                Data = tags
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<List<TagDto>>
            {
                Success = false,
                Message = "An error occurred while retrieving tags.",
                Errors = new List<string> { e.Message }
            });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TagDto), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<TagDto>), 500)]
    public async Task<ActionResult<ApiResponseDto<TagDto?>>> GetByIdAsync(int id)
    {
        try
        {
            var tags = await _tagService.GetByIdAsync(id);
            if (tags == null)
            {
                return NotFound(new ApiResponseDto<TagDto>
                {
                    Success = false,
                    Message = "Tag not found."
                });
            }

            return Ok(new ApiResponseDto<TagDto>
            {
                Success = true,
                Data = tags
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<TagDto>
            {
                Success = false,
                Message = "An error occurred while retrieving the tag.",
                Errors = new List<string> { e.Message }
            });
        }
    }

    [HttpGet("name/{name}")]
    [ProducesResponseType(typeof(TagDto), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<TagDto>), 500)]
    public async Task<ActionResult<ApiResponseDto<TagDto?>>> GetByNameAsync(string name)
    {
        try
        {
            var tags = await _tagService.GetByNameAsync(name);
            if (tags == null)
            {
                return BadRequest(new ApiResponseDto<TagDto>
                {
                    Success = false,
                    Message = "Tag not found."
                });
            }

            return Ok(new ApiResponseDto<TagDto>
            {
                Success = true,
                Data = tags
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<TagDto>
            {
                Success = false,
                Message = "An error occurred while retrieving the tag.",
                Errors = new List<string> { e.Message }
            });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(TagDto), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<TagDto>), 500)]
    public async Task<ActionResult<ApiResponseDto<TagDto>>> Create([FromBody] CreateTagDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDto<TagDto>
                {
                    Success = false,
                    Message = "Invalid input data."
                });
            }

            var tag = await _tagService.CreateAsync(dto);
            return Ok(new ApiResponseDto<TagDto>
            {
                Success = true,
                Message = "Tag created successfully.",
                Data = tag
            });
        }
        catch (ArgumentException e)
        {
            return BadRequest(new ApiResponseDto<TagDto>
            {
                Success = false,
                Message = e.Message
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<TagDto>
            {
                Success = false,
                Message = "An error occurred while creating the tag.",
                Errors = new List<string> { e.Message }
            });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TagDto), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<TagDto>), 500)]
    public async Task<ActionResult<ApiResponseDto<TagDto>>> UpdateAsync(int id, [FromBody] UpdateTagDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDto<TagDto>
                {
                    Success = false,
                    Message = "Invalid input data."
                });
            }

            var tag = await _tagService.UpdateAsync(id, dto);
            return Ok(new ApiResponseDto<TagDto>
            {
                Success = true,
                Message = "Tag updated successfully.",
                Data = tag
            });
        }
        catch (ArgumentException e)
        {
            return BadRequest(new ApiResponseDto<TagDto>
            {
                Success = false,
                Message = e.Message
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<TagDto>
            {
                Success = false,
                Message = "An error occurred while updating the tag.",
                Errors = new List<string> { e.Message }
            });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto), 200)]
    [ProducesResponseType(typeof(ApiResponseDto), 500)]
    public async Task<ActionResult<ApiResponseDto>> DeleteAsync(int id)
    {
        try
        {
            var deleted = await _tagService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Tag not found."
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Tag deleted successfully."
            });
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(new ApiResponseDto
            {
                Success = false,
                Message = e.Message
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto
            {
                Success = false,
                Message = "An error occurred while deleting the tag.",
                Errors = new List<string> { e.Message }
            });
        }
    }

    [HttpGet("popular")]
    [ProducesResponseType(typeof(List<TagDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<List<TagDto>>), 500)]
    public async Task<ActionResult<ApiResponseDto<List<TagDto>>>> GetPopularAsync([FromQuery] int count = 10)
    {
        try
        {
            var tags = await _tagService.GetPopularAsync(count);
            return Ok(new ApiResponseDto<List<TagDto>>
            {
                Success = true,
                Message = "Popular tags retrieved successfully.",
                Data = tags
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<List<TagDto>>
            {
                Success = false,
                Message = "An error occurred while retrieving popular tags.",
                Errors = new List<string> { e.Message }
            });
        }
    }
    
}