using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using BLL.Infrastructure;
using BLL.DTO;
using BLL.BusinessModels;
using DAL.EF;
using DAL.Entities;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class TaskServices:ITaskService
    {
        CatContext Database { get; set; }
        private readonly IMapper _mapper;
        public TaskServices(CatContext catContext, IMapper mapper)
        {
            _mapper = mapper;
            Database = catContext;
        }

        public List<CatDTO> GetCats(string attribute, string order, int? offset, int? limit)
        {
            if (attribute == null)
                attribute = "name";
            if ((attribute != "name") && (attribute != "color") &&
                (attribute != "tail_length") && (attribute != "whiskers_length"))
              throw new BLL.Infrastructure.ValidationException(@"The ""attribute"" parameter is not correct. 
                    Use ""name"" or ""color"" or ""tail_length"" or ""whiskers_length""", "");
            order = order?.ToLower();
            if (!String.IsNullOrEmpty(order) && (!String.Equals(order, "asc") && !String.Equals(order, "desc")))
                throw new BLL.Infrastructure.ValidationException(@"The ""order"" parameter is not correct. Use ""asc"" or ""desc""", "");
            if (offset != null)
            {
                if (offset >= Database.Cats.Count())
                    throw new BLL.Infrastructure.ValidationException(@"The ""offset"" >= cats count", "");
                else if (offset < 0)
                    throw new BLL.Infrastructure.ValidationException(@"The ""offset"" cannot be less 0", "");
            }
            else if (offset == null) offset = 0;
            if (limit != null)
            {                
                if (limit < 1)
                    throw new BLL.Infrastructure.ValidationException(@"The ""limit"" cannot be less 1", "");
            }
            List<Cat> cats = null;
            IQueryable<Cat> query = null;
            if(order == "desc")
            {
                if (attribute == "name")
                    query = Database.Cats.OrderByDescending(i => i.Name).Skip((int)offset);
                else if (attribute == "color")
                    query = Database.Cats.OrderByDescending(i => i.Color).Skip((int)offset);
                else if (attribute == "tail_length")
                    query = Database.Cats.OrderByDescending(i => i.TailLength).Skip((int)offset);
                else if (attribute == "whiskers_length")
                    query = Database.Cats.OrderByDescending(i => i.WhiskersLength).Skip((int)offset);                
            }
            else
            {
                if (attribute == "name")
                    query = Database.Cats.OrderBy(i => i.Name).Skip((int)offset);
                else if (attribute == "color")
                    query = Database.Cats.OrderBy(i => i.Color).Skip((int)offset);
                else if (attribute == "tail_length")
                    query = Database.Cats.OrderBy(i => i.TailLength).Skip((int)offset);
                else if (attribute == "whiskers_length")
                    query = Database.Cats.OrderBy(i => i.WhiskersLength).Skip((int)offset);
            }
            if(limit!=null)
                cats = query.Take((int)limit).ToList();
            else
                cats = query.ToList();
            if (cats == null)
            {
                throw new SelectException("No objects found ", "");                
            }
            return  _mapper.Map<List<Cat>, List<CatDTO>>(cats);            
        }

        public List<CatColorInfoDTO> Exercise1()
        {
            new Exercises().ProcessingExercise1(Database);
            return _mapper.Map<List<CatColorInfo>, List<CatColorInfoDTO>>(
                Database.CatColorInfos.Select(i => i).ToList());//тут делаю выборку для проверки привильности записанных данных
        }

        public CatStatDTO Exercise2()
        {
            new Exercises().ProcessingExercise2(Database);
            return _mapper.Map<CatStat, CatStatDTO>(
                Database.CatStats.Select(i => i).Single());//тут делаю выборку для проверки привильности записанных данных
        }

        public void AddCat(NewCatDTO newCatDTO)
        {   
            if(Database.Cats.Any(i=>i.Name == newCatDTO.Name))
                throw new BLL.Infrastructure.ValidationException("A cat with the same name already exists", "");
            Database.Cats.Add(_mapper.Map<NewCatDTO, Cat>(newCatDTO));
            Database.SaveChanges();
        }

        public string Ping()
        {
            return "Cats Service. Version 0.1";
        }

        


        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
