﻿using Microsoft.AspNetCore.Mvc;
using PvPet.Business.DTOs;
using PvPet.Business.Services.Contracts;

namespace PvPet.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GameLoopController : ControllerBase
{
    private readonly IPetService _petService;

    public GameLoopController(IPetService petService)
    {
        _petService = petService;
    }

    [HttpPatch("updatePetLocation")]
    public async Task<IActionResult> UpdatePetLocation([FromBody] PetDto update)
    {
        if (!await _petService.UpdateAsync(update))
        {
            return BadRequest();
        }

        var enemyLocations = (await _petService.QueryAsync(predicate: pet => pet.Id != update.Id))
            .Select(pet => pet.Location)
            .ToList();

        return Ok(enemyLocations);
    }
}