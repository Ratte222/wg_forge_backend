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
        List<CatDTO> GetCats(string ownerLogin);
        List<CatColorInfoDTO> Exercise1();
        CatStatDTO Exercise2();
        void AddCat(NewCatDTO newCatDTO, string ownerLogin);
        void EditCat(NewCatDTO newCatDTO, string ownerLogin);
        void DeleteCat(CatDTO catDTO, string ownerLogin);
        List<CatOwnerDTO> GetCatOwners();
        CatOwnerDTO GetCatOwner(string ownerLogin);
        void CheckCatInOwner(string catName, string ownerLogin);
        void AddCatPhoto(List<CatPhotoDTO> catPhotosDTO, string catName);
    }
}
