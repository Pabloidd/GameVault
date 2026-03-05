using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameVault.RequestModels
{
    public class CreatePublisherRequest
    {
        public string Company { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}