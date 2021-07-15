using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wg_forge_backend.Models
{
    public class AddOrEditCatResponseModel
    {
        public Dictionary<string, string> HexColor { get; set; } 
        public Dictionary<string, int> ReasoneAddCat { get; set; }
    }
}
