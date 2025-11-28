using bacalah.API.Models;
using bacalah.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bacalah.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    
    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryDto>), 200)]
    [ProducesResponseType(typeof(List<CategoryDto>), 400)]
    [ProducesResponseType(typeof(ApiResponseDto<List<CategoryDto>>), 500)]
    public async Task<ActionResult<ApiResponseDto<List<CategoryDto>>>> GetAll()
    {
        try
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(new ApiResponseDto<List<CategoryDto>>
            {
                Success = true,
                Data = categories
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<List<CategoryDto>>
            {
                Success = false,
                Message = "An error occurred while retrieving categories.",
                Errors = new List<string> { e.Message }
            });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryDto), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<CategoryDto>), 404)]
    [ProducesResponseType(typeof(ApiResponseDto<CategoryDto>), 500)]
    public async Task<ActionResult<ApiResponseDto<CategoryDto?>>> GetById(int id)
    {
        try
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new ApiResponseDto<CategoryDto>
                {
                    Success = false,
                    Message = "Category not found."
                });
            }

            return Ok(new ApiResponseDto<CategoryDto>
            {
                Success = true,
                Data = category
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<CategoryDto>
            {
                Success = false,
                Message = "An error occurred while retrieving the category.",
                Errors = new List<string> { e.Message }
            });
        }
    }

    [HttpGet("tree")]
    [ProducesResponseType(typeof(List<CategoryTreeDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<List<CategoryTreeDto>>), 500)]
    public async Task<ActionResult<ApiResponseDto<List<CategoryTreeDto>>>> GetTree()
    {
        try
        {
            var categories = await _categoryService.GetTreeAsync();
            return Ok(new ApiResponseDto<List<CategoryTreeDto>>
            {
                Success = true,
                Data = categories
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<List<CategoryTreeDto>>
            {
                Success = false,
                Message = "An error occurred while retrieving category tree.",
                Errors = new List<string> { e.Message }
            });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryDto), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<CategoryDto>), 400)]
    [ProducesResponseType(typeof(ApiResponseDto<CategoryDto>), 500)]
    public async Task<ActionResult<ApiResponseDto<CategoryDto>>> Create([FromBody] CreateCategoryDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDto<CategoryDto>
                {
                    Success = false,
                    Message = "Invalid data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }
            
            var category = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, new ApiResponseDto<CategoryDto>
            {
                Success = true,
                Message = "Category created successfully.",
                Data = category
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<List<CategoryTreeDto>>
            {
                Success = false,
                Message = "An error occurred while creating the category.",
                Errors = new List<string> { e.Message }
            });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CategoryDto), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<CategoryDto>), 400)]
    [ProducesResponseType(typeof(ApiResponseDto<CategoryDto>), 404)]
    [ProducesResponseType(typeof(ApiResponseDto<CategoryDto>), 500)]
    public async Task<ActionResult<ApiResponseDto<CategoryDto>>> Update(int id, [FromBody] UpdateCategoryDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDto<CategoryDto>
                {
                    Success = false,
                    Message = "Invalid data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var category = await _categoryService.UpdateAsync(id, dto);
            return Ok(new ApiResponseDto<CategoryDto>
            {
                Success = true,
                Message = "Category updated successfully.",
                Data = category
            });
        }
        catch (ArgumentException e)
        {
            return BadRequest(new ApiResponseDto<CategoryDto>
            {
                Success = false,
                Message = e.Message
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<List<CategoryTreeDto>>
            {
                Success = false,
                Message = "An error occurred while updating the category.",
                Errors = new List<string> { e.Message }
            });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ApiResponseDto), 404)]
    [ProducesResponseType(typeof(ApiResponseDto), 500)]
    public async Task<ActionResult<ApiResponseDto>> Delete(int id)
    {
        try
        {
            var canDelete = await _categoryService.CanDeleteAsync(id);
            if (!canDelete)
            {
                return BadRequest(new ApiResponseDto
                {
                    Success = false,
                    Message = "Category cannot be deleted because it has subcategories or associated items."
                });
            }

            var deleted = await _categoryService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new ApiResponseDto
                {
                    Success = false,
                    Message = "Category not found."
                });
            }

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Category deleted successfully."
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
            return StatusCode(500, new ApiResponseDto<List<CategoryTreeDto>>
            {
                Success = false,
                Message = "An error occurred while deleting the category.",
                Errors = new List<string> { e.Message }
            });
        }
    }

    [HttpGet("{id}/subcategories")]
    [ProducesResponseType(typeof(List<CategoryDto>), 200)]
    [ProducesResponseType(typeof(ApiResponseDto<List<CategoryDto>>), 500)]
    public async Task<ActionResult<ApiResponseDto<List<CategoryTreeDto>>>> GetSubcategories(int id)
    {
        try
        {
            var sub = await _categoryService.GetSubCategoriesAsync(id);
            return Ok(new ApiResponseDto<List<CategoryDto>>
            {
                Success = true,
                Data = sub
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<List<CategoryDto>>
            {
                Success = false,
                Message = "An error occurred while retrieving subcategories.",
                Errors = new List<string> { e.Message }
            });

        }
    }
    
}