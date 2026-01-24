using AutoMapper;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.AutoMappers
{
    public class AutoMapperSettings : Profile
    {
        public AutoMapperSettings()
        {
            //CreateMap<ソースClass, 目標Class>();
            CreateMap<AccountDTO, AccountModel>();
            CreateMap<AccountModel, AccountDTO>();
        }
    }
}
