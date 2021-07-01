using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using BLL.Infrastructure;
using BLL.DTO;
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
        public TaskServices()
        {
            Database = new CatContext();
        }

        public List<CatDTO> GetCats(string attribute, string order, int? offset, int? limit)
        {
            if ((attribute == null) && (attribute != "name") && (attribute != "color") &&
                (attribute != "tail_length") && (attribute != "whiskers_length"))
                throw new ValidationException(@"The ""attribute"" parameter is not correct. 
                    Use ""name"" or ""color"" or ""tail_length"" or ""whiskers_length""", "");
            order = order?.ToLower();
            if (!String.IsNullOrEmpty(order) && (!String.Equals(order, "asc") && !String.Equals(order, "desc")))
                throw new ValidationException(@"The ""order"" parameter is not correct. Use ""asc"" or ""desc""", "");
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
            List<CatDTO> catsDTO = new List<CatDTO>();
            if(cats == null)
            {
                throw new SelectException("No objects found ", "");                
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Cat, CatDTO>());
            var mapper = new Mapper(config);
            foreach (Cat cat in cats)
            {
                catsDTO.Add(mapper.Map<Cat, CatDTO>(cat));
            }
            return catsDTO;
            //return mapper.Map<Cat, CatDTO>(Database.Cats.First());
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
