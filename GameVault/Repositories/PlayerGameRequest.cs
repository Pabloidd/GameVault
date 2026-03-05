using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameVault.Repositories
{
    public class PlayerGameRequest
    {
        public string Nickname { get; set; } = string.Empty;
        public string GameTitle { get; set; } = string.Empty;
    }
}