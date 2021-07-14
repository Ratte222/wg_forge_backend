using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;
namespace BLL.Interfaces
{
    public interface IAccount
    {
        AccountModelDTO Authenticate(LoginModelDTO loginModelDTO);
        void Registration(RegisterModelDTO registerModelDTO);
    }
}
