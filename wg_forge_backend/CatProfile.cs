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
            CreateMap<Cat, CatDTO>()
                .ForMember("CatPhotosDTO", opt => opt.MapFrom(scr => scr.CatPhotos));
            CreateMap<CatDTO, Cat>();
            CreateMap<CatColorInfo, CatColorInfoDTO>();
            CreateMap<CatStat, CatStatDTO>();
            CreateMap<NewCatDTO, Cat>();
            CreateMap<NewCatDTO, CatDTO>();
            CreateMap<CatOwner, CatOwnerDTO>();
            CreateMap<CatOwnerDTO, CatOwner>();
            CreateMap<RegisterModelDTO, CatOwner>();
            CreateMap<CatOwner, AccountModelDTO>();
            CreateMap<CatPhotoDTO, CatPhoto>();
            CreateMap<CatPhoto, CatPhotoDTO>();
        }
    }
}
