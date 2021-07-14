using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using BLL.Infrastructure;
using BLL.DTO;
using BLL.BusinessModels;
using DAL.Interface;
using DAL.Entities;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class TaskServices:ITaskService
    {
        //CatContext Database { get; set; }
        private IRepository<Cat> _repoCat;
        private IRepository<CatColorInfo> _repoCatColorInfo;
        private IRepository<CatStat> _repoCatStat;
        private IRepository<CatOwner> _repoCatOwners;
        private readonly IMapper _mapper;
        public TaskServices(IRepository<Cat> repoCat, IRepository<CatColorInfo> repoCatColorInfo,
            IRepository<CatStat> repoCatStat, IRepository<CatOwner> repoCatOwners,  IMapper mapper)
        {
            _mapper = mapper;
            this._repoCat = repoCat;
            this._repoCatColorInfo = repoCatColorInfo;
            this._repoCatStat = repoCatStat;
            this._repoCatOwners = repoCatOwners;
        }

        public List<CatDTO> GetCats(string OwnerLogin)
        {
            CatOwner catOwner = _repoCatOwners.GetAll_Queryable().Include(i => i.Cats)
                .Single(i => i.Login.ToLower() == OwnerLogin.ToLower());
            if (catOwner == null)
                throw new SelectException("Сould not find the owner. Please log in again.");
            return _mapper.Map<List<Cat>, List<CatDTO>>(catOwner.Cats);
        }

        public List<CatDTO> GetAllCats(string attribute, string order, int? offset, int? limit)
        {
            if (attribute == null)
                attribute = "Name";
            if ((attribute != "Name") && (attribute != "Color") &&
                (attribute != "TailLength") && (attribute != "WhiskersLength"))
              throw new BLL.Infrastructure.ValidationException(@"The ""attribute"" parameter is not correct. " +
                    @"Use ""Name"" or ""Color"" or ""TailLength"" or ""WhiskersLength""");
            order = order?.ToLower();
            if (!String.IsNullOrEmpty(order) && (!String.Equals(order, "asc") && !String.Equals(order, "desc")))
                throw new BLL.Infrastructure.ValidationException(@"The ""order"" parameter is not correct. Use ""asc"" or ""desc""");
            bool orderBy = false;
            if (String.Equals(order, "desc"))
                orderBy = true;
            if (offset != null)
            {
                if (offset >= _repoCat.GetAll_Queryable().Count())
                    throw new BLL.Infrastructure.ValidationException(@"The ""offset"" >= cats count");
                else if (offset < 0)
                    throw new BLL.Infrastructure.ValidationException(@"The ""offset"" cannot be less 0");
            }
            else if (offset == null) offset = 0;
            if (limit != null)
            {                
                if (limit < 1)
                    throw new BLL.Infrastructure.ValidationException(@"The ""limit"" cannot be less 1");
            }
            List<Cat> cats = null;
            IQueryable<Cat> query = _repoCat.GetAll_Queryable().Include(c => c.CatOwners).OrderBy(attribute, orderBy).Skip((int)offset);
            if(limit!=null)
                cats = query.Take((int)limit).ToList();
            else
                cats = query.ToList();
            if (cats == null)
            {
                throw new SelectException("No objects found ");                
            }
            return  _mapper.Map<List<Cat>, List<CatDTO>>(cats);            
        }

        public List<CatOwnerDTO> GetCatOwners()
        {
            List<CatOwner> catOwners = _repoCatOwners.GetAll_Queryable()
                .Include(u=>u.Cats).OrderBy(i => i.Name).ToList();
            return _mapper.Map<List<CatOwner>, List<CatOwnerDTO>>(catOwners);
        }

        public List<CatColorInfoDTO> Exercise1()
        {
            new Exercises().ProcessingExercise1(_repoCat, _repoCatColorInfo);
            return _mapper.Map<List<CatColorInfo>, List<CatColorInfoDTO>>(
                _repoCatColorInfo.GetAll_Enumerable().ToList());//тут делаю выборку для проверки привильности записанных данных
        }

        public CatStatDTO Exercise2()
        {
            new Exercises().ProcessingExercise2(_repoCat, _repoCatStat);
            return _mapper.Map<CatStat, CatStatDTO>(
                _repoCatStat.GetAll_Enumerable().Single());//тут делаю выборку для проверки привильности записанных данных
        }

        public void AddCat(NewCatDTO newCatDTO, string OwnerLogin)
        {
            CatOwner catOwner = _repoCatOwners.GetAll_Queryable().Single(i => i.Login.ToLower() == OwnerLogin.ToLower());
            if (catOwner == null)
                throw new SelectException("Сould not find the owner. Please log in again.");
            if(_repoCat.GetAll_Queryable().Any(i=>i.Name.ToLower() == newCatDTO.Name.ToLower()))
                throw new BLL.Infrastructure.ValidationException("A cat with the same name already exists");
            Cat cat = _mapper.Map<NewCatDTO, Cat>(newCatDTO);
            _repoCat.Create(cat);
            catOwner.Cats.Add(cat);
            _repoCatOwners.Update(catOwner);
        }

        public void EditCat(NewCatDTO catDTO, string OwnerLogin)
        {            
            if (!_repoCat.GetAll_Queryable().Any(i => i.Name.ToLower() == catDTO.Name.ToLower()))
                throw new ValidationException("A cat with the same name already not exists");            
            if (!_repoCatOwners.GetAll_Queryable().AsNoTracking().Include(i => i.Cats)
                .Single(i => i.Login.ToLower() == OwnerLogin.ToLower())
                .Cats.Any(i => i.Name.ToLower() == catDTO.Name.ToLower()))
                throw new ValidationException("You are not the owner of this cat");
            _repoCat.Update(_mapper.Map<NewCatDTO, Cat>(catDTO));
        }

        public void DeleteCat(CatDTO catDTO, string OwnerLogin)
        {
            if (!_repoCatOwners.GetAll_Queryable().AsNoTracking().Include(i => i.Cats)
                .Single(i => i.Login.ToLower() == OwnerLogin.ToLower())
                .Cats.Any(i => i.Name.ToLower() == catDTO.Name.ToLower()))
                throw new ValidationException("You are not the owner of this cat");
            _repoCat.Delete(_mapper.Map<CatDTO, Cat>(catDTO));
        }

        public string Ping()
        {
            return "Cats Service. Version 0.1";
        }
    }
}
