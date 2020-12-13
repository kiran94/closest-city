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

    /// <summary>
    /// Access City Information
    /// </summary>
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

        /// <summary>
        /// Gets Cities based on Name
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>a list of cities</returns>
        [HttpGet]
        [ActionName("ByName")]
        [Produces("application/json")]
        public async Task<IEnumerable<Worldcity>> ByName(
            [FromQuery] string name, 
            CancellationToken token = default)
        {
            return await sql.Set<Worldcity>().Where(x => x.City == name).ToListAsync(token);
        }

        /// <summary>
        /// Gets Citities within radius (km) of the given latitude,longitude.
        /// </summary>
        /// <param name="latitude">the latitude of the reference point.</param>
        /// <param name="longitude">The longitude of the reference point.</param>
        /// <param name="radiusKm">The radius in KM to search.</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>A list of cities</returns>
        [HttpGet]
        [ActionName("ByLocation")]
        [Produces("application/json")]
        public async Task<IEnumerable<dynamic>> ByLocation(
            [FromQuery] double latitude = 51.533007,
            [FromQuery] double longitude = -0.188142,
            [FromQuery] double radiusKm = 10_000,
            CancellationToken token = default)
        {
            return await this.ByLocation(new Coordinate(longitude, latitude), radiusKm, token);
        }

        /// <summary>
        /// Gets Citities within radius (km) of a given coordinate
        /// </summary>
        /// <param name="coordinate">the coordinate (reference point)</param>
        /// <param name="radiusKm">The radius in KM to search</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>a list of cities</returns>
        [HttpPost]
        [ActionName("ByLocation")]
        [Produces("application/json")]
        public async Task<IEnumerable<dynamic>> ByLocation(
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
