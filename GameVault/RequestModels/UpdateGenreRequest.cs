using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameVault.DTO
{
    public class UpdateGenreRequest
    {
        public string OldGenreName { get; set; } = string.Empty;
        public string NewGenreName { get; set; } = string.Empty;
    }
}