using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Mappings
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash));
                // Map PasswordHash to Password

            CreateMap<AccountDto, Account>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)); // Map Password to PasswordHash

        }
    }
}
