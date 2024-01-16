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
    private readonly IShopItemService _shopItemService;


    public PetController(IUserService userService, IPetService petService, IShopItemService shopItemService)
    {
        _userService = userService;
        _service = petService;
        _shopItemService = shopItemService;
    }

    [HttpPost()]
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

    [HttpPost("Buy")]
    public async Task<IActionResult> Buy([FromBody]string id)
    {
        var id2 = Guid.Parse(id);
        var item = await _shopItemService.GetShopItemById(id2);
        if (item == null)  return BadRequest();

        var user = await _userService.QuerySingleAsync(
            include: u => u.Include(u => u.Pet!),
            predicate: u => u.Username == HttpContext.GetUsername()
        );

        if (user.Pet.Gold < item.Price)
            return BadRequest();

        user.Pet.Gold -= item.Price;
        user.Pet.Food += item.Food;
        user.Pet.Armor += item.Armor;
        user.Pet.Attack += item.Attack;
        user.Pet.AttackSpeed += item.AttackSpeed;
        user.Pet.Crit += item.Crit;
        user.Pet.Hp += item.Hp;
        

        await _service.UpdateAsync(user.Pet);
        await _shopItemService.RemoveItem(item);

        return Ok();
    }

}
