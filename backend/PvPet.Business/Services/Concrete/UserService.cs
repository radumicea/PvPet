using System.Net;
using System.Security.Claims;
using AutoMapper;
using Crop360.Business.Services.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PvPet.Business.DTOs;
using PvPet.Business.Exceptions;
using PvPet.Business.Services.Contracts;
using PvPet.Data.Entities;

namespace PvPet.Business.Services.Concrete;

public class UserService : BaseService<User, UserDto>, IUserService
{
    public UserService(DbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<ClaimsPrincipal> GetClaimsAsync(UserDto request)
    {
        var user = await QuerySingleAsync(predicate: dto => dto.Username == request.Username);
        if (user == null)
            throw new ResponseStatusException(HttpStatusCode.Unauthorized);

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new ResponseStatusException(HttpStatusCode.Unauthorized);

        var nameClaim = new Claim(ClaimTypes.Name, user.Username);

        return new ClaimsPrincipal(new ClaimsIdentity(new[] { nameClaim }, CookieAuthenticationDefaults.AuthenticationScheme));
    }

    public override async Task<Guid> AddAsync(UserDto user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        return await base.AddAsync(user);
    }

    public async Task UpdateRestockTime(int elapsedSeconds)
    {
        await _entitiesSet.ExecuteUpdateAsync(u => u.SetProperty(i => i.SecondsToRestockShop, i => i.SecondsToRestockShop - elapsedSeconds));
    }
}