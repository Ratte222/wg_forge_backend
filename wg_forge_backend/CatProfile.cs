using System;
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

            CreateMap<CatOwner, CatOwnerDTO>()
                .ForMember("Cats", opt => opt.MapFrom(scr => scr.CatsAndOwners));
                //.ForMember("Cats", opt => opt.MapFrom(scr => scr.CatsAndOwners.Select<Cat>(i => i.Cat)));
            CreateMap<CatsAndOwners, CatDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(scr => scr.Cat.Name))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(scr => scr.Cat.Color))
                .ForMember(dest => dest.TailLength, opt => opt.MapFrom(scr => scr.Cat.TailLength))
                .ForMember(dest => dest.WhiskersLength, opt => opt.MapFrom(scr => scr.Cat.WhiskersLength))
                .ForMember(dest => dest.CatPhotosDTO, opt => opt.MapFrom(scr => scr.Cat.CatPhotos));
            CreateMap<CatOwnerDTO, CatOwner>();
            CreateMap<RegisterModelDTO, CatOwner>();
            CreateMap<CatOwner, AccountModelDTO>();
            CreateMap<CatPhotoDTO, CatPhoto>();
            CreateMap<CatPhoto, CatPhotoDTO>();
                //.ForMember(dest=>dest.CatPhotoName, opt=>opt.MapFrom(scr=> "CatImages/"+ scr.CatPhotoName));
        }
    }
}
