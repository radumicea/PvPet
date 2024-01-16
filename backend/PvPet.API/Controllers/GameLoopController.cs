using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PvPet.API.Extensions;
using PvPet.Business.DTOs;
using PvPet.Business.Services.Contracts;

namespace PvPet.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GameLoopController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPetService _petService;
    private readonly IItemOnMapService _itemOnMapService;
    private readonly IShopItemService _shopItemService;

    public GameLoopController(
        IUserService userService,
        IPetService petService,
        IItemOnMapService itemOnMapService, IShopItemService shopItemService)
    {
        _userService = userService;
        _petService = petService;
        _itemOnMapService = itemOnMapService;
        _shopItemService = shopItemService;
    }

    [HttpPatch("updateGameState")]
    public async Task<IActionResult> UpdateGameState([FromBody] PetDto update)
    {
        if (HttpContext.User?.Identity?.IsAuthenticated == false)
        {
            return Unauthorized();
        }
        var user = await _userService.QuerySingleAsync(
            include: u => u.Include(u => u.Pet!),
            predicate: u => u.Username == HttpContext.GetUsername()
            );

        if (!await _petService.UpdateAsync(new PetDto { Id = user!.Pet!.Id, Location = update.Location }))
        {
            return BadRequest();
        }

        var pet = await _petService.QuerySingleAsync(
            include: p => p.Include(p => p.PetsFights!).Include(u => u.ShopItems!).Include(p => p.Items!),
            predicate: p => p.UserId == user.Id
            );

        var enemyLocations = (await _petService.QueryAsync(predicate: pet => pet.Id != user!.Pet!.Id))
            .Select(pet => pet.Location)
            .ToList();

        var itemLocations = (await _itemOnMapService.QueryAsync())
            .Select(i => i.Location)
            .ToList();

        var shopItems = (await _shopItemService.GetShopItems(pet.Id))
            .ToList();

        return Ok(new { pet, enemyLocations, itemLocations });
    }
}
