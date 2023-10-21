using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PvPet.Business.DTOs;
using PvPet.Business.Services.Contracts;

namespace PvPet.API.Controllers;

[ApiController]
[Route("books")]
public class BookController : ControllerBase
{
    private readonly IBookService _service;

    public BookController(IBookService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _service.QueryAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var book = await _service.QuerySingleAsync(predicate: b => b.Id == id);
        return book is not null
            ? Ok(book)
            : NotFound(id);
    }

    [HttpGet("{id}/categories")]
    public async Task<IActionResult> GetCategories([FromRoute] Guid id)
    {
        var book = await _service.QuerySingleAsync(
            predicate: b => b.Id == id,
            include: b => b.Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
            );

        return book is not null
            ? Ok(book.BookCategories.Select(bc => bc.Category))
            : NotFound(id);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] BookDto model)
    {
        var id = await _service.AddAsync(model);

        return id != Guid.Empty
            ? Ok(id)
            : BadRequest();
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] BookDto update)
    {
        var ok = await _service.UpdateAsync(update);

        return ok
            ? Ok()
            : BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] BookDto model)
    {
        var ok = await _service.DeleteAsync(model);

        return ok
            ? Ok()
            : BadRequest();
    }

    [HttpPost("batch")]
    public async Task<IActionResult> Add([FromBody] IEnumerable<BookDto> models)
    {
        var ok = await _service.AddRangeAsync(models);

        return ok
            ? Ok()
            : BadRequest();
    }

    [HttpPatch("batch")]
    public async Task<IActionResult> Update([FromBody] IEnumerable<BookDto> updates)
    {
        var ok = await _service.UpdateRangeAsync(updates);

        return ok
            ? Ok()
            : BadRequest();
    }

    [HttpDelete("batch")]
    public async Task<IActionResult> Delete([FromBody] IEnumerable<BookDto> models)
    {
        var ok = await _service.DeleteRangeAsync(models);

        return ok
            ? Ok()
            : BadRequest();
    }
}