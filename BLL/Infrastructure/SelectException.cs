using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Infrastructure
{
    public class SelectException : Exception
    {
        public string Property { get; protected set; }
        public SelectException(string message, string prop="") : base(message)
        {
            Property = prop;
        }
    }
}
