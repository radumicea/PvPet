using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PvPet.Business.DTOs;
using PvPet.Business.Exceptions;
using PvPet.Business.Extensions;
using PvPet.Business.Services.Contracts;
using PvPet.Data.Entities;

namespace PvPet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private IAccountService _accountService;
        private IPetService _petService;

        public PetController(IAccountService accountService, IPetService petService)
        {
            _accountService = accountService;
            _petService = petService;
        }

        private async Task<AccountDto?> GetAccount()
        {
            var account = await _accountService.QuerySingleAsync(predicate: dto => dto.Username == HttpContext.GetAccount());
            if (account == null)
                throw new ResponseStatusException(HttpStatusCode.Unauthorized);
        }

        [Authorize]
        public async Task<ActionResult<PetDto>> CreatePet([FromBody]string name)
        {
            var account = await GetAccount();

            var pet = new PetDto(){}

        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pet>> GetPet()
        {
            var account = await GetAccount();
        }


    }
}
