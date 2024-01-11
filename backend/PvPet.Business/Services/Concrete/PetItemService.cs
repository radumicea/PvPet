using AutoMapper;
using Crop360.Business.Services.Generic;
using Microsoft.EntityFrameworkCore;
using PvPet.Business.DTOs;
using PvPet.Business.Services.Contracts;
using PvPet.Data.Entities;

namespace PvPet.Business.Services.Concrete;

public class PetItemService : BaseService<PetItem, PetItemDto>, IPetItemService
{
    public PetItemService(DbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
