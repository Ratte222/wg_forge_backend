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
            if(offset!=null)    
            {
                if(offset >= Database.Cats.Count())
                    throw new BLL.Infrastructure.ValidationException(@"The ""offset"" >= cats count", "");
                else if (offset < 0)
                    throw new BLL.Infrastructure.ValidationException(@"The ""offset"" cannot be less 0", "");
            }
            if (limit != null)
            {                
                if (limit < 1)
                    throw new BLL.Infrastructure.ValidationException(@"The ""limit"" cannot be less 1", "");
            }
            List<Cat> cats = null;
            SqlParameter sqlLimit = new SqlParameter("@limit", limit),
                    sqlOffset = new SqlParameter("@offset", offset);
            if ((limit != null)&&(offset != null))
            {
                //cats = Database.Cats.FromSqlRaw($"SELECT * FROM cats ORDER BY {attribute} {order}").Skip((int)offset).Take((int)limit).ToList();
                cats = Database.Cats.FromSqlRaw($"SELECT * FROM cats ORDER BY {attribute} {order} " +
                    $"OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY", sqlOffset, sqlLimit)/*.Skip((int)offset)*/.ToList();
            }
            else if(offset != null)
            {                
                cats = Database.Cats.FromSqlRaw($"SELECT * FROM cats ORDER BY {attribute} {order} " +
                    $"OFFSET @offset ROWS", sqlOffset)/*.Skip((int)offset)*/.ToList();
            }
            else if(limit != null)
            {
                cats = Database.Cats.FromSqlRaw($"SELECT * FROM cats " +
                    $"ORDER BY {attribute} {order} OFFSET 0 ROWS FETCH NEXT @limit ROWS ONLY", sqlLimit)/*.Take((int)limit)*/.ToList();
            }
            else
            {
                cats = Database.Cats.FromSqlRaw($"SELECT * FROM cats ORDER BY {attribute} {order}",
                     order).ToList();
            }
            if(cats == null)
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
