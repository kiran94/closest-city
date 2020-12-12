namespace ClosestCity.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using ClosestCity.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("[controller]")]
    public class CityController : ControllerBase
    {
        private readonly closestcityContext sql;
        private readonly ILogger<CityController> _logger;
        
        public CityController(closestcityContext sql, ILogger<CityController> logger)
        {
            this.sql = sql;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Worldcity> Get()
        {
            return sql.Set<Worldcity>().Where(x => x.City == "London");
        }
    }
}
