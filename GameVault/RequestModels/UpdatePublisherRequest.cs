using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameVault.RequestModels
{
    public class UpdatePublisherRequest
    {
        public string Company { get; set; } = string.Empty;
        public string NewCountry { get; set; } = string.Empty;
    }
}