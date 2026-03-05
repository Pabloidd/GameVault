using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameVault.RequestModels
{
    public class CreateGameRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}