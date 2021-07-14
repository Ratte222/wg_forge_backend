using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ITaskService
    {
        string Ping();
        List<CatDTO> GetAllCats(string attribute, string order, int? offset, int? limit);
        List<CatDTO> GetCats(string OwnerLogin);
        List<CatColorInfoDTO> Exercise1();
        CatStatDTO Exercise2();
        void AddCat(NewCatDTO newCatDTO, string OwnerLogin);
        void EditCat(NewCatDTO newCatDTO, string OwnerLogin);
        void DeleteCat(CatDTO catDTO, string OwnerLogin);
        List<CatOwnerDTO> GetCatOwners();
    }
}
