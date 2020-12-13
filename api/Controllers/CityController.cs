namespace ClosestCity.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ClosestCity.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using NetTopologySuite.Geometries;

    [ApiController]
    [Route("[controller]/[action]")]
    public class CityController : ControllerBase
    {
        private readonly ApiContext sql;
        private readonly ILogger<CityController> logger; 
        private readonly IConfiguration configuration;
        private readonly GeometryFactory geometryFactory;
        
        public CityController(ApiContext sql, ILogger<CityController> logger, IConfiguration configuration, GeometryFactory geometryFactory)
        {
            this.sql = sql;
            this.logger = logger;
            this.configuration = configuration;
            this.geometryFactory = geometryFactory;
        }

        [HttpGet]
        [ActionName("ByName")]
        [Produces("application/json")]
        public async Task<IEnumerable<Worldcity>> ByName(
            [FromQuery] string name, 
            CancellationToken token = default)
        {
            return await sql.Set<Worldcity>().Where(x => x.City == name).ToListAsync(token);
        }

    
        [HttpGet]
        [ActionName("ByLocation")]
        [Produces("application/json")]
        public async Task<IEnumerable<dynamic>> ByLocation(
            [FromQuery] double latitude = 51.533007,
            [FromQuery] double longitude = -0.188142,
            [FromQuery] double radiusKm = 10_000,
            CancellationToken token = default)
        {
            return await this.ByLocation2(new Coordinate(longitude, latitude), radiusKm, token);
        }

        [HttpGet]
        [ActionName("ByLocation2")]
        [Produces("application/json")]
        public async Task<IEnumerable<dynamic>> ByLocation2(
            [FromBody] Coordinate coordinate,
            [FromQuery] double radiusKm = 100,
            CancellationToken token = default)
        {
            var point = this.geometryFactory.CreatePoint(coordinate);
            return await sql.Set<WorldcitiesGeometry>()
                .Where(x => x.Geom.Distance(point) < radiusKm)
                .Take(100)
                .Select(x => new
                {
                    x.City,
                    x.AdminName,
                    x.Lat,
                    x.Lng,
                    DistanceKm = x.Geom.Distance(point)
                })
                .OrderByDescending(x => x.DistanceKm)
                .ToListAsync(token);
        }
    }
}
