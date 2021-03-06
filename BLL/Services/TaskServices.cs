using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using BLL.Infrastructure;
using BLL.DTO;
using BLL.BusinessModels;
using DAL.Entities;
using AutoMapper;
using DAL.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BLL.Services
{
    public class TaskServices:ITaskService
    {
        //CatContext Database { get; set; }
        private ICatService _repoCat;
        private IRepository<CatColorInfo> _repoCatColorInfo;
        private IRepository<CatStat> _repoCatStat;
        private IRepository<CatPhoto> _repoCatPhoto;
        private ICatOwnerService _repoCatOwners;
        private readonly IMapper _mapper;
        private AppSettings _appSettings;
        public TaskServices(ICatService repoCat, IRepository<CatColorInfo> repoCatColorInfo,
            IRepository<CatStat> repoCatStat, ICatOwnerService repoCatOwners,  IMapper mapper,
            IRepository<CatPhoto> repoCatPhoto, IOptions<AppSettings> appSettings)
        {
            _mapper = mapper;
            this._repoCat = repoCat;
            this._repoCatColorInfo = repoCatColorInfo;
            this._repoCatStat = repoCatStat;
            this._repoCatPhoto = repoCatPhoto;
            this._repoCatOwners = repoCatOwners;
            _appSettings = appSettings.Value;
        }

        public List<CatDTO> GetCats(string ownerUserName)//do not work
        {
            CatOwner catOwner = _repoCatOwners.GetAll_Queryable()
                .FirstOrDefault(i => i.UserName.ToLower() == ownerUserName.ToLower());
            if (catOwner == null)
                throw new SelectException("Сould not find the owner. Please log in again.");
            
            return _mapper.Map<List<CatsAndOwners>, List<CatDTO>>(catOwner.CatsAndOwners);
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
            IQueryable<Cat> query = _repoCat.GetAll_Queryable().OrderBy(attribute, orderBy).Skip((int)offset);
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
                .OrderBy(i => i.UserName).ToList();
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
                _repoCatStat.GetAll_Enumerable().FirstOrDefault());//тут делаю выборку для проверки привильности записанных данных
        }

        public void AddCat(NewCatDTO newCatDTO, string ownerUserName)
        {
            newCatDTO.CheckColors(_appSettings);
            newCatDTO.CheckReasoneAddCat(_appSettings);
            CatOwner catOwner = _repoCatOwners.GetAll_Queryable().
                FirstOrDefault(i => i.UserName.ToLower() == ownerUserName.ToLower());
            if (catOwner == null)
                throw new SelectException("Сould not find the owner. Please log in again.");
            if(catOwner.CatsAndOwners.Any(i=>i.Cat.Name.ToLower() == newCatDTO.Name.ToLower()))
                throw new BLL.Infrastructure.ValidationException("A cat with the same name already exists");
            Cat cat = _mapper.Map<NewCatDTO, Cat>(newCatDTO);
            //_repoCat.Create(cat);
            CatsAndOwners catsAndOwners = new CatsAndOwners
            {
                Cat = cat
            };
            catOwner.CatsAndOwners.Add(catsAndOwners);
            //catOwner.Cats.Add(cat);
            catOwner.CatPoints += _appSettings.ReasoneDeleteCat[newCatDTO.ReasoneAddCat.ToLower()];
            _repoCatOwners.Update(catOwner);
        }

        public void EditCat(NewCatDTO catDTO, string ownerUserName)
        {
            catDTO.CheckColors(_appSettings);
            //if (!_repoCat.GetAll_Queryable().Any(i => i.Name.ToLower() == catDTO.Name.ToLower()))
            //    throw new ValidationException("A cat with the same name already not exists");            
            long catId = _repoCatOwners.GetAll_Queryable()
                .First(i => i.UserName.ToLower() == ownerUserName.ToLower())
                .CatsAndOwners.First(i => i.Cat.Name.ToLower() == catDTO.Name.ToLower()).Cat.Id;
            if(catId < 1)    
                throw new ValidationException("A cat with the same name already not exists");
            Cat cat = _mapper.Map<NewCatDTO, Cat>(catDTO);
            cat.Id = catId;
            _repoCat.Update(cat);
        }

        public void DeleteCat(CatDTO catDTO, string ownerUserName)
        {
            catDTO.CheckReasoneAddCat(_appSettings);

            if (!_repoCatOwners.GetAll_Queryable().AsNoTracking()
                .First(i => i.UserName.ToLower() == ownerUserName.ToLower())
                .CatsAndOwners.Any(i => i.Cat.Name.ToLower() == catDTO.Name.ToLower()))
                //if(!catOwner.Cats.Any(i => i.Name.ToLower() == catDTO.Name.ToLower()))    
                throw new ValidationException("You are not the owner of this cat");
            CatOwner catOwner = _repoCatOwners.GetAll().Include(i => i.CatsAndOwners)
                .ThenInclude(i=>i.Cat)
                .First(i => i.UserName.ToLower() == ownerUserName.ToLower());
            Cat cat = _mapper.Map<CatDTO, Cat>(catDTO);
            CatsAndOwners catsAndOwners = catOwner.CatsAndOwners.First(i => i.Cat.Name.ToLower() == catDTO.Name.ToLower());
            cat.Id = catsAndOwners.Cat.Id;
            _repoCat.Delete(cat);
            catOwner.CatsAndOwners.Clear();
            catOwner.CatPoints += _appSettings.ReasoneDeleteCat[catDTO.ReasoneDeleteCat.ToLower()];
            _repoCatOwners.Update(catOwner);
        }

        public string Ping()
        {
            return "Cats Service. Version 0.1";
        }

        public CatOwnerDTO GetCatOwner(string ownerUserName)
        {
            CatOwner catOwners = _repoCatOwners.GetAll_Queryable()
                .First(i=>i.UserName.ToLower() == ownerUserName.ToLower());
            //CatOwner catOwners_ = _repoCatOwners.GetAll().AsNoTracking().Include(i=>i.CatsAndOwners)
            //    .ThenInclude(i=>i.Cat).ThenInclude(i=>i.CatPhotos)
            //    .Single(i => i.Email.ToLower() == ownerEmail.ToLower());
            return _mapper.Map<CatOwner, CatOwnerDTO>(catOwners);
        }

        public void AddCatPhoto(List<CatPhotoDTO> catPhotosDTO, string catName)
        {
            Cat cat = _repoCat.GetAll_Queryable().AsNoTracking().Include(i=>i.CatPhotos).First(i => i.Name.ToLower() == catName.ToLower());
            cat.CatPhotos.AddRange(_mapper.Map<List<CatPhotoDTO>, IEnumerable<CatPhoto>>(catPhotosDTO));
            _repoCat.Update(cat);
        }

        public void CheckCatInOwner(string catName, string ownerEmail)
        {
            if (!_repoCatOwners.GetAll_Queryable().AsNoTracking()
                .First(i => i.UserName.ToLower() == ownerEmail.ToLower())
                .CatsAndOwners.Any(i => i.Cat.Name.ToLower() == catName.ToLower()))
                throw new ValidationException("You are not the owner of this cat");
        }
        public void CheckPhotoExistInCat(string userName, string photoName)
        {
            if (!_repoCatPhoto.GetAll_Queryable().Include(i => i.Cat).ThenInclude(i => i.CatsAndOwners)
                .ThenInclude(i => i.CatOwner).First(i => i.CatPhotoName.ToLower() == photoName.ToLower())
                .Cat.CatsAndOwners.Any(i=>i.CatOwner.UserName.ToLower() == userName.ToLower()))
                throw new ValidationException("You are not the owner of this cat");
        }
    }
}
