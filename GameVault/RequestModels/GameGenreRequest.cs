using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameVault.RequestModels
{
    
    public class GameGenreRequest
    {
        public string GameName { get; set; } = string.Empty;
        public string GenreName { get; set; } = string.Empty;
    }
}