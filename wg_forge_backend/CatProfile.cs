﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Entities;
using BLL.DTO;

namespace wg_forge_backend
{
    public class CatProfile : Profile
    {
        public CatProfile()
        {
            CreateMap<Cat, CatDTO>();
            CreateMap<CatColorInfo, CatColorInfoDTO>();
            CreateMap<CatStat, CatStatDTO>();
            CreateMap<NewCatDTO, Cat>();
        }
    }
}