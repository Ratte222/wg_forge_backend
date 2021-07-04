using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ITaskService
    {
        string Ping();
        List<CatDTO> GetCats(string attribute, string order, int? offset, int? limit);
        List<CatColorInfoDTO> Exercise1();
        CatStatDTO Exercise2();
        void AddCat(string jsonString);
        void Dispose();
    }
}
