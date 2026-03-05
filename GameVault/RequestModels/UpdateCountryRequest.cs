using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameVault.DTO
{
    public class UpdateCountryRequest
    {
        public string OldCountryName { get; set; } = string.Empty;
        public string NewCountryName { get; set; } = string.Empty;
    }
}