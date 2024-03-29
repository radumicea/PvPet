﻿using System.Security.Claims;
using Crop360.Business.Services.Generic;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Services.Contracts
{
    public interface IUserService : IBaseService<User, UserDto>
    {
        Task<ClaimsPrincipal> GetClaimsAsync(UserDto request);
        Task UpdateFirebaseToken(UserDto user);
    }
}
