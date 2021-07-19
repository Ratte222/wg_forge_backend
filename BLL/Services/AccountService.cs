using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using BLL.DTO;
using DAL.Entities;
using DAL.Interface;
using AutoMapper;
using BLL.Infrastructure;
namespace BLL.Services
{
    public class AccountService:IAccountService
    {
        private ICatOwnerService _repoCatOwners;
        private readonly IMapper _mapper;

        public AccountService(ICatOwnerService repoCatOwners, IMapper mapper)
        {
            _repoCatOwners = repoCatOwners;
            _mapper = mapper;
        }

        //public AccountModelDTO Authenticate(LoginModelDTO loginModelDTO)
        //{
        //    loginModelDTO.Password = Crypt.GetHashSHA512(loginModelDTO.Password);
        //    return _mapper.Map<CatOwner, AccountModelDTO>(
        //        _repoCatOwners.GetAll_Queryable().FirstOrDefault(i=>
        //        (i.Email.ToLower() == loginModelDTO.Email) &&
        //        (i.Password == loginModelDTO.Password)));
        //}
        //public void Registration(RegisterModelDTO registerModelDTO)
        //{
        //    if (_repoCatOwners.GetAll_Queryable().Any(i => i.Email.ToLower() == registerModelDTO.Email.ToLower()))
        //        throw new ValidationException("This login already exists ");
        //    CatOwner catOwner = _mapper.Map<RegisterModelDTO, CatOwner>(registerModelDTO);
        //    catOwner.Password = Crypt.GetHashSHA512(catOwner.Password);
        //    catOwner.Role = AccountRole.CatOwner;
        //    _repoCatOwners.Create(catOwner);
        //}
    }
}
