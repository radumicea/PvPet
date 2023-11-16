using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Crop360.Business.Services.Generic;
using Microsoft.EntityFrameworkCore;
using PvPet.Business.DTOs;
using PvPet.Business.Services.Contracts;
using PvPet.Data.Entities;

namespace PvPet.Business.Services.Concrete
{
    public class PetService : BaseService<Pet,PetDto>, IPetService
    {
        public PetService(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
