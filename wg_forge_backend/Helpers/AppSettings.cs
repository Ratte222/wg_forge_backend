using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wg_forge_backend.Helpers
{
    public class AppSettings
    {
        public string Issuer { get; set; }    // издатель токена
        public string Audience { get; set; }  // потребитель токена
        public int Lifetime { get; set; }     // время жизни токена - 5 минут
        public string Secret { get; set; }    // ключ для шифрации
    }
}
