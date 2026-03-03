using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameVault.Data
{
    public interface IMariaDbConnector
    {
        Task CreateConnection();
    }
}