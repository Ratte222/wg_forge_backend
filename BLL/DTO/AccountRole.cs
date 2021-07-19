using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    //public enum ACCOUNT_ROLE
    //{
    //    Admin,
    //    User,
    //    Guest
    //}

    public class AccountRole
    {
        public const string Admin = "Admin"; 
        public const string CatOwner = "CatOwner"; 
        public const string User = "User";

        public static readonly string[] Roles = { Admin, CatOwner, User };
    }
}
