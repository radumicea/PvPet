using System.Net;
using System.Security.Claims;
using System.Xml;
using AutoMapper;
using Crop360.Business.Services.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PvPet.Business.DTOs;
using PvPet.Business.Exceptions;
using PvPet.Business.Services.Contracts;
using PvPet.Data.Entities;

namespace PvPet.Business.Services.Concrete;

public class AccountService : BaseService<Account,AccountDto> ,IAccountService
{
    public AccountService(DbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<ClaimsPrincipal> GetClaimsAsync(AccountDto request)
    {
        var user = await QuerySingleAsync(predicate: dto => dto.Username == request.Username);
        if (user == null)
        {
            throw new ResponseStatusException(HttpStatusCode.Unauthorized);
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password)) 
            throw new ResponseStatusException(HttpStatusCode.Unauthorized);

        var nameClaim = new Claim(ClaimTypes.Name, user.Username);

        return new ClaimsPrincipal(new ClaimsIdentity(new []{nameClaim}, CookieAuthenticationDefaults.AuthenticationScheme));
    }

    public async Task CreateAccountAsync(AccountDto account)
    {
        account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
        await AddAsync(account);
    }
}