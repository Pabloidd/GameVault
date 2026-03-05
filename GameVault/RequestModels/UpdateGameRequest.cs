using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameVault.RequestModels
{
    public class UpdateGameRequest
    {
        public string Title { get; set; } = string.Empty;
        public string NewCompany { get; set; } = string.Empty;
        public decimal NewWeight { get; set; }
        public DateTime NewReleaseDate { get; set; }
    }
}