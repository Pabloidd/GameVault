using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameVault.RequestModels
{
    public class UpdatePlayerRequest
    {
        public string Nickname { get; set; } = string.Empty;
        public string NewEmail { get; set; } = string.Empty;
        public int NewLevel { get; set; }
    }
}