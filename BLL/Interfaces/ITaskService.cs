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
        List<CatDTO> GetCats(string ownerUserName);
        List<CatColorInfoDTO> Exercise1();
        CatStatDTO Exercise2();
        void AddCat(NewCatDTO newCatDTO, string ownerUserName);
        void EditCat(NewCatDTO newCatDTO, string ownerUserName);
        void DeleteCat(CatDTO catDTO, string ownerUserName);
        List<CatOwnerDTO> GetCatOwners();
        CatOwnerDTO GetCatOwner(string ownerUserName);
        void CheckCatInOwner(string catName, string ownerUserName);
        void AddCatPhoto(List<CatPhotoDTO> catPhotosDTO, string catName);
        void CheckPhotoExistInCat(string userName, string photoNmae);
    }
}
