using Microsoft.AspNetCore.Mvc;
using PvPet.API.Extensions;
using PvPet.Business.DTOs;
using PvPet.Business.Services.Contracts;

namespace PvPet.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PetController : ControllerBase
{
    private readonly IPetService _service;

    public PetController(IPetService petService)
    {
        _service = petService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] PetDto model)
    {
        var pet = new PetDto(model.Name!, HttpContext.GetUserId());
        var id = await _service.AddAsync(pet);

        return id != Guid.Empty
            ? Ok(pet)
            : BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var pet = await _service.QuerySingleAsync(predicate: p => p.UserId == HttpContext.GetUserId());
        return Ok(pet);
    }
}
