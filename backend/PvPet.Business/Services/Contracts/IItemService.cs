﻿using System.Security.Claims;
using Crop360.Business.Services.Generic;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Services.Contracts
{
    public interface IItemService : IBaseService<Item, ItemDto>
    {
    }
}
