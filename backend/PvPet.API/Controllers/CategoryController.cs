using Microsoft.AspNetCore.Mvc;
using PvPet.Business.DTOs;
using PvPet.Business.Services.Contracts;

namespace PvPet.API.Controllers;

[ApiController]
[Route("categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CategoryDto model)
    {
        var id = await _service.AddAsync(model);

        return id != Guid.Empty
            ? Ok(id)
            : BadRequest();
    }
}