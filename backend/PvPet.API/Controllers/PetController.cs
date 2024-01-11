using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PvPet.API.Extensions;
using PvPet.Business.DTOs;
using PvPet.Business.Services.Contracts;

namespace PvPet.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PetController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPetService _service;

    public PetController(IUserService userService, IPetService petService)
    {
        _userService = userService;
        _service = petService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] PetDto model)
    {
        var user = await _userService.QuerySingleAsync(predicate: u => u.Username == HttpContext.GetUsername());
        var pet = PetDto.New(model.Name!, user!.Id);
        var id = await _service.AddAsync(pet);

        return id != Guid.Empty
            ? Ok(pet)
            : BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var pet = await _service.QuerySingleAsync(include: p => p.Include(p => p.User!), predicate: p => p.User!.Username == HttpContext.GetUsername());
        return Ok(pet);
    }
}
