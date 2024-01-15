using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PvPet.API.Extensions;
using PvPet.Business.DTOs;
using PvPet.Business.Services.Contracts;

namespace PvPet.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPetService _petService;

        public UserController(IUserService userService, IPetService petService)
        {
            _userService = userService;
            _petService = petService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserDto user)
        {
            var id = await _userService.AddAsync(user);

            var pet = PetDto.New("pet1", id);
            await _petService.AddAsync(pet);
            return id != Guid.Empty
            ? Ok()
            : BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto user)
        {
            var claims = await _userService.GetClaimsAsync(user);
            await HttpContext.SignInAsync(claims);
            await _userService.UpdateFirebaseToken(user);

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }
}
