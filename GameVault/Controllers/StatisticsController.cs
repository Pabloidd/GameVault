using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameVault.Models;
using GameVault.Repositories;
using Microsoft.AspNetCore.Mvc;
using GameVault.RequestModels;


namespace GameVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : BaseController
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public StatisticsController(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        [HttpGet]
        public Task<ActionResult<DatabaseStatistics>> GetDatabaseStatistics()
        {
            return ExecuteGetAsync(() => _statisticsRepository.GetDatabaseStatisticsAsync());
        }
    }
}