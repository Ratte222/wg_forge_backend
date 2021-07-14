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
    public class Account:IAccount
    {
        private IRepository<CatOwner> _repoCatOwners;
        private readonly IMapper _mapper;

        public Account(IRepository<CatOwner> repoCatOwners, IMapper mapper)
        {
            _repoCatOwners = repoCatOwners;
            _mapper = mapper;
        }

        public AccountModelDTO Authenticate(LoginModelDTO loginModelDTO)
        {
            loginModelDTO.Password = Crypt.GetHashSHA512(loginModelDTO.Password);
            return _mapper.Map<CatOwner, AccountModelDTO>(
                _repoCatOwners.GetAll_Queryable().FirstOrDefault(i=>
                (i.Login.ToLower() == loginModelDTO.Login) &&
                (i.Password == loginModelDTO.Password)));
        }
        public void Registration(RegisterModelDTO registerModelDTO)
        {
            if (_repoCatOwners.GetAll_Queryable().Any(i => i.Login.ToLower() == registerModelDTO.Login.ToLower()))
                throw new ValidationException("This login already exists ");
            CatOwner catOwner = _mapper.Map<RegisterModelDTO, CatOwner>(registerModelDTO);
            catOwner.Password = Crypt.GetHashSHA512(catOwner.Password);
            catOwner.Role = ACCOUNT_ROLE.User.ToString();
            _repoCatOwners.Create(catOwner);
        }
    }
}
