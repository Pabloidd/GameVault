using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameVault.Models
{
    public class DatabaseStatistics
    {
        public int TotalGames { get; set; }
        public int TotalPlayers { get; set; }
        public int TotalGenres { get; set; }
        public int TotalPublishers { get; set; }
        public int TotalCountries { get; set; }
    }
}