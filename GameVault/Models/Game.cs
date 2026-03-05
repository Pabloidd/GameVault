using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameVault.Models
{
    public class Game
{
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public DateTime release_date { get; set; }  //  как в БД, временный фикс
}
}