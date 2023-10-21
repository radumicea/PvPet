using Microsoft.AspNetCore.Mvc;
using PvPet.Business.DTOs;
using PvPet.Business.Services.Contracts;

namespace PvPet.API.Controllers;

[ApiController]
[Route("book-categories")]
public class BookCategoryController : ControllerBase
{
    private readonly IBookCategoryService _service;

    public BookCategoryController(IBookCategoryService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] BookCategoryDto model)
    {
        var id = await _service.AddAsync(model);

        return id != Guid.Empty
            ? Ok(id)
            : BadRequest();
    }
}