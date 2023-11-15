using Microsoft.AspNetCore.Mvc;
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
        var id = await _service.AddAsync(model);

        return id != Guid.Empty
            ? Ok(id)
            : BadRequest();
    }
}
